// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace PullToRefresh
{
    public partial class CitiesViewController : UITableViewController
    {
        List<String> lista;
        Dictionary<string, List<string>> dick;
        UIRefreshControl refreshControl;

        #region Constructors
        public CitiesViewController(IntPtr handle) : base(handle)
        {

        }
        #endregion

        #region ControllerLifeCycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //Mostrar las ciudades que tiene por default la app

            dick = CitiesManager.SharedInstance.GetDefaultCities();

            InitializeComponents();

            /*
            foreach (KeyValuePair<string, List<string>> dic in dick)
            {
                Console.WriteLine("Key: {0}", dic.Key);
                foreach (string str in dic.Value){
                    Console.WriteLine("Value: {0}", str);
                }
            }
            */

            lista = new List<String> { "Deni", "Negrette", "Garavito", "Gigi", "Sofia" };


        }
        #endregion

        #region TableViewDataSource
        public override nint NumberOfSections(UITableView tableView)
        {
            return dick.Count;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return dick.Keys.ElementAt(int.Parse(section.ToString()));
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return dick.Keys.ToArray();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            List<String> lst = dick.Values.ElementAt(int.Parse(section.ToString()));
            return lst.Count();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            List<String> lst = dick.Values.ElementAt(int.Parse(indexPath.Section.ToString()));
            var cell = tableView.DequeueReusableCell("idCell", indexPath);
            cell.TextLabel.Text = $"{lst.ElementAt(indexPath.Row)}";
            return cell;
        }
        #endregion

        public void UpdateTable(Object sender, EventArgs e){
            CitiesManager.SharedInstance.FetchiCities();
            TableView.RefreshControl.EndRefreshing();
        }

        #region CitiesManagerEvents
        void CitiesManager_CitiesFetched (object sender, CitiesEventArgs e){
            dick = e.Cities;

            InvokeOnMainThread(() =>
            {
                TableView.ReloadData();
                TableView.RefreshControl.EndRefreshing();
                refreshControl.EndRefreshing();
            });
        }

        void CitiesManager_FetchCitiesFailed(object sender, CitiesEventArgsFailed e)
        {
            InvokeOnMainThread(() =>
            {
                ShowMessage("ERROR", e.Failed, NavigationController);
            });
        }
        #endregion

        #region Functionality
        void InitializeComponents()
        {
            CitiesManager.SharedInstance.CitiesFetched += CitiesManager_CitiesFetched;
            CitiesManager.SharedInstance.FetchCitiesFailed += CitiesManager_FetchCitiesFailed;
            dick = CitiesManager.SharedInstance.GetDefaultCities();

            refreshControl = new UIRefreshControl();
            refreshControl.ValueChanged += UpdateTable;

            TableView.RefreshControl = refreshControl;
        }
        #endregion

        void ShowMessage(string title, string message, UIViewController fromViewController)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            fromViewController.PresentViewController(alert, true, null);
        }
    }
}
