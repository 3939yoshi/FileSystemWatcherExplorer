
# FileSystemWatcher Explorer C#.NET
**FileSystemWatcher �N���X�̓���𒲂ׂ܂�**

### 1. �T�v
FileSystemWatcher�́A�t�@�C�� �V�X�e���̕ύX�ʒm��ҋ@���A�f�B���N�g���܂��̓f�B���N�g�����̃t�@�C�����ύX���ꂽ�Ƃ��ɃC�x���g�𔭐������܂��B

�쐬�����A�v���P�[�V�����Ŏ��X�w��f�B���N�g���Ƀt�@�C���������ꂽ���Ƃ����o�ł��Ȃ���肪�������Ă���̂ŋ����m�F�p�A�v���P�[�V�������쐬�����B


Created��Changed�̃C�x���g�������񔭐�����ꍇ������̂ł��̓�����m�F����B

IsFileExistsAndUnlocked()�֐��Ńt�@�C�������݂ƃ��b�N����Ă��Ȃ��Ƃ��m�F����B


```
        // �t�@�C�������݂��Ĕ񃍃b�N���B
        private static bool IsFileExistsAndUnlocked(string path, out string falseMessage)
        {
            falseMessage = string.Empty;
            FileStream? stream = null;
            if (! File.Exists(path))
            {
                falseMessage = "File.Exiets() is false.";
                return false; 
            }
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch(DirectoryNotFoundException e)
            {
                falseMessage = e.Message;
                return false;
            }
            catch(FileNotFoundException e)
            {
                falseMessage = e.Message;
                return false;
            }
            catch(IOException e)
            {
                falseMessage = e.Message;
                return false;
            }
            catch(Exception e)
            {
                falseMessage = e.Message;
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return true;
        }

```


### 2. ��
Microsoft Visual Studio Community 2022 C#
.NET + WPF + Prism


### 3.����(�Q�l)

A.txt�𑀍삵���ꍇ�̌��ʁB

**NotifyFiles��FileName DirectoryName LastWrite��ݒ莞**

| ��   |�Ώۃf�B���N�g��<br>A.TXT���<br>�Ȃ�|�Ώۃf�B���N�g��<br>A.TXT���<br>0�o�C�g|�Ώۃf�B���N�g��<br>A.TXT���<br>1�o�C�g�ȏ� |
|-|-|-|
|Notepad<br>���O��t����0�o�C�g<br>�ۑ�|Created<br>Deleted<br>Created<br>Canged|Changed|Changed|
|Notepad<br>���O��t���Đ��o�C�g<br>�ۑ�|Created<br>Deleted<br>Created<br>Canged|Changed|Changed|
|Rename                   |Renamed|�~|�~|
|�h���b�O �ړ� 0�o�C�g    |Created|�~|�~|
|�h���b�O �ړ� ���o�C�g   |Created|�~|�~|
|�h���b�O �R�s�[ 0�o�C�g  |Created<br>Changed|�~|�~|
|�h���b�O �R�s�[ ���o�C�g |Created<br>Changed<br>Changed|�~|�~|
|�\��t�� �ړ� 0�o�C�g    |Created|�~|�~|
|�\��t�� �ړ� ���o�C�g   |Created|�~|�~|
|�\��t�� �R�s�[ 0�o�C�g  |Created<br>Changed|�~|�~|
|�\��t�� �R�s�[ ��0�o�C�g|Created<br>Changed<br>Changed|�~|�~|
|Ctrl + V �ړ� 0�o�C�g    |Created<br>Changed<br>Changed|�~|�~|
|Ctrl + V �ړ� ���o�C�g   |Changed<br>Changed|�~|�~|
|Ctrl + V �R�s�[ 0�o�C�g  |Changed<br>Changed|�~|�~|
|Ctrl + V �R�s�[ ���o�C�g |Changed<br>Changed<br>Changed|�~|�~|



### �t�^1 


