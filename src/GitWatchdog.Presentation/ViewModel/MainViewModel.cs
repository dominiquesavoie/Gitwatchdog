using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GitWatchdog.Presentation.Command;
using GitWatchdog.Presentation.Extensions;
using GitWatchdog.Presentation.Helpers;
using GitWatchdog.Presentation.Model;
using GitWatchdog.Presentation.Services;
using Plugin.Connectivity;
using SQLite;

namespace GitWatchdog.Presentation.ViewModel
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private readonly IPlatformProvider _platformProvider;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ISubject<string> _pipedOutput = new Subject<string>();

        private readonly ISubject<Unit> _forceRefreshSubject = new Subject<Unit>();

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            DispatcherHelper.DefaultDispatcherScheduler.Schedule(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get { return _isRunning; }
            private set
            {
                _isRunning = value; 

                OnPropertyChanged(nameof(IsRunning));
                RefreshCommand.RaiseCanExecuteChangedIfPossible();
            }
        }

        public MainViewModel()
        {
            Start();
            // Since this application only has 1 view currently, I wont bother disposing my observables...
            //TODO If the app is expanded, consider added it to be disposable.
            
            Observable.Timer(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(15))
                .SelectUnit()
                .Merge(_forceRefreshSubject)
                .WhereIsConnected()
                .Where(_ => !IsRunning)
                .ObserveOn(DispatcherHelper.DefaultDispatcherScheduler)
                .Select(_ => _items.ToArray())
                .Where(items => items.Any())
                .Do(_ => IsRunning = true)
                .SelectMany(async items =>
                {
                    _pipedOutput.OnNext($"{DateTime.Now:G}: Starting to update local repos.{Environment.NewLine}");

                    foreach (var item in items)
                    {
                        await HandleRepoUpdate(item);
                    }
                    _pipedOutput.OnNext($"{DateTime.Now:G}: All updates ended.{Environment.NewLine}");
                    return Unit.Default;
                })
                .CatchError()
                .ObserveOn(DispatcherHelper.DefaultDispatcherScheduler)
                .Do(_ => IsRunning = false)
                .Subscribe();

            _pipedOutput
                .Where(text => !string.IsNullOrWhiteSpace(text))
                .Do(Console.Write)
                //Write to displayable log here
                .Do(text => Log += text)
                .CatchError()
                .Subscribe();
        }

        private async Task HandleRepoUpdate(Item item)
        {
            _pipedOutput.OnNext($"{DateTime.Now:G}: Starting to update {item.Name}.{Environment.NewLine}");
            await RunProcess(PrepareStartInfo(item));
            _pipedOutput.OnNext($"{DateTime.Now:G}: Update {item.Name} ended.{Environment.NewLine}");
            _pipedOutput.OnNext($"{DateTime.Now:G} Started to purge local branches from {item.Name}.{Environment.NewLine}");
            await PurgeBranchGoneFromRemote(item.Path);
            _pipedOutput.OnNext($"{DateTime.Now:G} Purge local branches from {item.Name} ended.{Environment.NewLine}");
        }

        private string _log;

        public string Log
        {
            get { return _log; }
            set
            {
                _log = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Log)));
            }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new Command.Command(_ => _forceRefreshSubject.OnNext(), _ => !IsRunning));
            }
        }

        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                return _browseCommand ?? (_browseCommand = new Command.Command(_ =>
                {
                    //TODO: Move this to a service in GitWatchdog.WPF
                    //var dialog = new FolderBrowserDialog
                    //{
                    //    ShowNewFolderButton = false,
                    //    Description = "Select local git repository",
                    //};

                    //var result = dialog.ShowDialog();

                    //if(result != DialogResult.OK)
                    //{
                    //    return;
                    //}

                    //AddNewRepo.ExecuteCommandIfPossible(dialog.SelectedPath);
                }));
            }
        }

        private Task RunProcess(ProcessStartInfo info)
        {
            return Task.Run(async () =>
            {
                var process = new Process
                {
                    StartInfo = info,
                };
                
                if (!process.Start())
                {
                    return;
                }
                process.WaitForExit();
                var errorOutput = await process.StandardError.ReadToEndAsync();
                var standardOutput = await process.StandardOutput.ReadToEndAsync();

                var text = errorOutput.Split('\n')
                    .Concat(standardOutput.Split('\n'));

                _pipedOutput.OnNext(string.Join("\n", text));
            });
        }

        private ProcessStartInfo PrepareStartInfo(Item item)
        {
            return new ProcessStartInfo("git", "fetch -p")
            {
                WorkingDirectory = item.Path,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                ErrorDialog = false,
                UseShellExecute = false
            };
        }

        private ObservableCollection<Item> _items = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
            }
        }

        private async Task<SQLiteAsyncConnection> GetConnection()
        {
            var fileName = Path.Combine(AppDataFolderHelper.AppDataFolder, "data.db");
            Directory.CreateDirectory(AppDataFolderHelper.AppDataFolder);
            var connection = new SQLiteAsyncConnection(fileName);

            await connection.CreateTableAsync<Item>();

            return connection;
        }


        private async void Start()
        {
            try
            {
                var items = await Task.Run<IList<Item>>(async () =>
                {
                    var connection = await GetConnection();

                    return await connection.Table<Item>().ToListAsync();
                });
                

                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private ICommand _deleteRepo;

        public ICommand DeleteRepo
        {
            get
            {
                return _deleteRepo ?? (_deleteRepo = new AsyncCommand(o =>
                {
                    var item = o as Item;

                    Items.Remove(item);

                    return Task.Run(async () =>
                    {
                        var connection = await GetConnection();

                        await connection.DeleteAsync(item);
                    });
                }));
            }
        }

        private string _gitHubRepoUrl;

        public string GitHubRepoUrl
        {
            get { return _gitHubRepoUrl; }
            set
            {
                _gitHubRepoUrl = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GitHubRepoUrl)));
            }
        }

        private async Task PurgeBranchGoneFromRemote(string gitPath)
        {
            var processInfo = _platformProvider.GetTerminal();
            processInfo.WorkingDirectory = gitPath;

            var process = Process.Start(processInfo);

            if (process == null)
            {
                return;
            }

            await process.StandardInput.WriteLineAsync("git branch -vv");
            await process.StandardInput.WriteLineAsync("exit");
            await process.StandardInput.FlushAsync();

            var content = await process.StandardOutput.ReadToEndAsync();

            var branches = content.Replace("\r", "").Split('\n')
                .Where(p => !p.StartsWith("*"))
                .Where(p => p.Contains(": gone]"))
                .Select(p => p.Trim().Split(' ').FirstOrDefault())
                .ToArray();

            if (!branches.Any())
            {
                // There is nothing to do, exiting
                return;
            }

            process = Process.Start(processInfo);

            if (process == null)
            {
                return;
            }

            foreach (var branch in branches)
            {
                _pipedOutput.OnNext($"{DateTime.Now:G} deleting {branch}");
                await process.StandardInput.WriteLineAsync($"git branch -df {branch}");
            }

            await process.StandardInput.WriteLineAsync("exit");
            await process.StandardInput.FlushAsync();

            content = await process.StandardOutput.ReadToEndAsync();

            _pipedOutput.OnNext($"Git output for branch deletion: {Environment.NewLine}");
            _pipedOutput.OnNext(content);
        }

        private ICommand _addNewRepo;

        public ICommand AddNewRepo
        {
            get
            {
                return _addNewRepo ?? (_addNewRepo = new AsyncCommand(param =>
                {
                    var text = param as string;

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        return Task.FromResult(Unit.Default);
                    }
                    var directory = new DirectoryInfo(text);
                    var name = directory.Name;

                    GitHubRepoUrl = string.Empty;

                    var item = new Item()
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        Path = text
                    };

                    Items.Add(item);

                    return Task.Run(async () =>
                    {
                        var connection = await GetConnection();

                        await connection.InsertAsync(item);

                        // Todo add a InvokeAsync
                        DispatcherHelper.DefaultDispatcherScheduler.Schedule(async () =>
                        {

                            IsRunning = true;

                            try
                            {
                                if (CrossConnectivity.Current.IsConnected &&
                                    await CrossConnectivity.Current.IsRemoteReachable("http://www.google.com"))
                                {
                                    await HandleRepoUpdate(item);
                                }
                            }
                            catch
                            {
                                // error occurs
                            }
                            finally
                            {
                                IsRunning = false;
                            }
                        });
                       
                    });
                }));
            }
        }

    }
}
