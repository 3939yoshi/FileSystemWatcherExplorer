using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FileSystemWatcherExplorer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScrollViewer? _listBoxScrollViewer = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            (listBox1.ItemsSource as INotifyCollectionChanged).CollectionChanged += new NotifyCollectionChangedEventHandler(listBox_CollectionChanged);

        }
        private void listBox_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ToEnd();
            //this.listBox.ScrollIntoView(this.listBox.Items[this.listBox.Items.Count - 1]);
        }
        private void ToEnd()
        {
            if(_listBoxScrollViewer == null)
            {
                Border? border = VisualTreeHelper.GetChild(listBox1, 0) as Border;
                if (border != null)
                {
                    _listBoxScrollViewer = border.Child as ScrollViewer;
                }
            }
            _listBoxScrollViewer?.ScrollToEnd();
        }
    }
}