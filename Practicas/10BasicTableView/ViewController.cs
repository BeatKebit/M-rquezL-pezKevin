using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace BasicTableView
{
    public partial class ViewController : UIViewController, IUITableViewDelegate, IUITableViewDataSource
    {
        List<String> lista;

        #region Constructors
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region ControllerLifeCycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            lista = new List<String> { "Deni", "Negrette", "Garavito", "Gigi", "Sofia" };

            tableView.DataSource = this;
            tableView.Delegate = this;
        }
        #endregion

        #region UITableViewDataSource
        [Export("numberOfSectionsInTableView:")]
        public nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return lista.Count;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("basicCell", indexPath);
            cell.TextLabel.Text = $"{lista[indexPath.Row]}";
            return cell;
        }
        #endregion

        #region UserInteractions
        partial void btnAdd(NSObject sender)
        {
            ShowMessage(NavigationController);
        }
        #endregion

        void ShowMessage(UIViewController fromViewController)
        {
            var alert = UIAlertController.Create("Select multiplication table:", "", UIAlertControllerStyle.Alert);
            for (int i = 1; i < 11; i++)
            {
                alert.AddAction(UIAlertAction.Create(i.ToString(), UIAlertActionStyle.Default, (UIAlertAction obj) => ShowMessageLimit(int.Parse(obj.Title))));
            }
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Default, null));
            fromViewController.PresentViewController(alert, true, null);
        }

        void ShowMessageLimit(int x)
        {
            //int y = 0;
            var alert = UIAlertController.Create("Select limit for table:", "", UIAlertControllerStyle.Alert);
            alert.AddTextField((UITextField obj) => obj.Text = "");
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (UIAlertAction obj) => populateTable(x, int.Parse(alert.TextFields[0].Text))));
            PresentViewController(alert, true, null);
        }

        void populateTable(int x, int y){
            lista = new List<string>();
            for (int i = 0; i <= y;i++){
                lista.Add(x + " * " + i + " = " + (x*i));
            }
            InvokeOnMainThread(() =>
            {
                tableView.ReloadData();
            });

        }
    }
}
