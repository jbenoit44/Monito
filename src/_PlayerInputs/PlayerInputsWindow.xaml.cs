using Dynamo.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Monito
{
    /// <summary>
    /// Interaction logic for PlayerInputsWindow.xaml
    /// </summary>
    public partial class PlayerInputsWindow : Window
    {

        public PlayerInputsWindow()
        {
            InitializeComponent();
        }

        void click_ResetAll(object sender, RoutedEventArgs e)
        {
            InputAction.Text = "ResetAll";
        }

        void click_ResetSelected(object sender, RoutedEventArgs e)
        {
            InputAction.Text = "ResetSelected";
        }

        void click_SetSelectedAsInput(object sender, RoutedEventArgs e)
        {
            InputAction.Text = "SetSelectedAsInput";
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            var dynWindow = this.Owner;
            var vm = dynWindow.DataContext as DynamoViewModel;
            string guid = "" + ((Button)sender).Tag;
            var VMU = new ViewModelUtils(vm, dynWindow);
            VMU.ZoomToObject(guid);
        }

        void button_MouseEnter(object sender, RoutedEventArgs e)
        {
            highlightNode.Text = "" + ((Button)sender).Tag;
        }

        void button_MouseLeave(object sender, RoutedEventArgs e)
        {
            unhighlightNode.Text = "" + ((Button)sender).Tag;
        }
    }
}