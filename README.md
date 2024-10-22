
# FileSystemWatcher Explorer C#.NET
**FileSystemWatcher クラスの動作を調べます**

### 1. 概要
FileSystemWatcherは、ファイル システムの変更通知を待機し、ディレクトリまたはディレクトリ内のファイルが変更されたときにイベントを発生させます。

CreatedやChangedのイベントが複数回発生する場合があるのでその動作を確認する。


### 2. 環境
Microsoft Visual Studio Community 2022 C#
.NET + WPF + Prism


### 3.結果(参考)

A.txtを操作した場合の結果。

**NotifyFilesはFileName DirectoryName LastWriteを設定時**

| ★   |対象ディレクトリ<br>A.TXT状態<br>なし|対象ディレクトリ<br>A.TXT状態<br>0バイト|対象ディレクトリ<br>A.TXT状態<br>1バイト以上 |
|-|-|-|
|Notepad<br>名前を付けて0バイト<br>保存|Created<br>Deleted<br>Created<br>Canged|Changed|Changed|
|Notepad<br>名前を付けて数バイト<br>保存|Created<br>Deleted<br>Created<br>Canged|Changed|Changed|
|Rename                   |Renamed|×|×|
|ドラッグ 移動 0バイト    |Created|×|×|
|ドラッグ 移動 数バイト   |Created|×|×|
|ドラッグ コピー 0バイト  |Created<br>Changed|×|×|
|ドラッグ コピー 数バイト |Created<br>Changed<br>Changed|×|×|
|貼り付け 移動 0バイト    |Created|×|×|
|貼り付け 移動 数バイト   |Created|×|×|
|貼り付け コピー 0バイト  |Created<br>Changed|×|×|
|貼り付け コピー 数0バイト|Created<br>Changed<br>Changed|×|×|
|Ctrl + V 移動 0バイト    |Created<br>Changed<br>Changed|×|×|
|Ctrl + V 移動 数バイト   |Changed<br>Changed|×|×|
|Ctrl + V コピー 0バイト  |Changed<br>Changed|×|×|
|Ctrl + V コピー 数バイト |Changed<br>Changed<br>Changed|×|×|



### 付録1 


