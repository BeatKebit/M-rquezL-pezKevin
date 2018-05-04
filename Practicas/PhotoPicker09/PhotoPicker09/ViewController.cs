using System;
using Photos;
using UIKit;
using Foundation;
using AVFoundation;

namespace PhotoPicker09
{
    public partial class ViewController : UIViewController, IUIImagePickerControllerDelegate
    {
        #region Variables

        UITapGestureRecognizer editTapGesture;
        UITapGestureRecognizer tapTapGesture;


        #endregion

        #region Constructor
        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region LifeCicle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            InitializeComponents();
        }
        #endregion

        #region UserInteractions

        void ShowOptions(UITapGestureRecognizer gesture)
        {
            var alert = new UIAlertController();

            alert.AddAction(UIAlertAction.Create("Open library", UIAlertActionStyle.Default, TryOpenLibrary));
            alert.AddAction(UIAlertAction.Create("Camera", UIAlertActionStyle.Default, CameraSelected));
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            PresentViewController(alert, true, null);

        }

        void TryOpenLibrary(UIAlertAction obj)
        {
            Console.WriteLine("photo");

            if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.PhotoLibrary))
            {
                //print msg 
            }
            CheckPhotoLibraryAuthorizationStatus(PHPhotoLibrary.AuthorizationStatus);


            {
                var imagePicker = new UIImagePickerController
                {
                    SourceType = UIImagePickerControllerSourceType.PhotoLibrary
                };
                PresentViewController(imagePicker, true, null);

            }

        }

        void CheckPhotoLibraryAuthorizationStatus(PHAuthorizationStatus authorizationStatus)
        {
            switch (authorizationStatus)
            {
                case PHAuthorizationStatus.NotDetermined:
                    //Pedir permiso
                    PHPhotoLibrary.RequestAuthorization(CheckPhotoLibraryAuthorizationStatus);
                    break;
                case PHAuthorizationStatus.Restricted:
                    InvokeOnMainThread(() =>
                    {
                        ShowMessage("Its is resticted", "", NavigationController);
                    });
                    break;
                case PHAuthorizationStatus.Denied:
                    InvokeOnMainThread(() =>
                    {
                        ShowMessage("It is denied", "", NavigationController);
                    });
                    break;
                case PHAuthorizationStatus.Authorized:
                    //Open photo library
                    InvokeOnMainThread(() =>
                    {
                        var imagePicker = new UIImagePickerController
                        {
                            SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                            Delegate = this
                        };
                        PresentViewController(imagePicker, true, null);
                    });
                    break;
                default:
                    break;

            }
        }

        void CheckCameraAuthorizationStatus(AVAuthorizationStatus authorizationStatus)
        {
            switch (authorizationStatus)
            {
                case AVAuthorizationStatus.NotDetermined:
                    //Pedir permiso
                    authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
                    CheckCameraAuthorizationStatus(authorizationStatus);
                    break;
                case AVAuthorizationStatus.Restricted:
                    InvokeOnMainThread(() =>
                    {
                        ShowMessage("Its is resticted", "", NavigationController);
                    });
                    break;
                case AVAuthorizationStatus.Denied:
                    InvokeOnMainThread(() =>
                    {
                        ShowMessage("It is denied", "", NavigationController);
                    });
                    break;
                case AVAuthorizationStatus.Authorized:
                    //Open photo library
                    InvokeOnMainThread(() =>
                    {
                        var imagePicker = new UIImagePickerController
                        {
                            SourceType = UIImagePickerControllerSourceType.Camera,
                            Delegate = this
                        };
                        PresentViewController(imagePicker, true, null);
                    });
                    break;
                default:
                    break;

            }
        }

        void CameraSelected(UIAlertAction obj)
        {
            Console.WriteLine("camera");

            if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
            {
                ShowMessage("Error", "Check if it is available.", NavigationController);
            }else{
                CheckPhotoLibraryAuthorizationStatus(PHPhotoLibrary.AuthorizationStatus);
                {
                    var imagePicker = new UIImagePickerController
                    {
                        SourceType = UIImagePickerControllerSourceType.PhotoLibrary
                    };
                    PresentViewController(imagePicker, true, null);
                }
            }

        }
        void InitializeComponents()
        {
            LblEdit.Hidden = true;
            LblTap.Hidden = true;

            editTapGesture = new UITapGestureRecognizer(ShowOptions) { Enabled = true };
            ProfileView.AddGestureRecognizer(editTapGesture);

            tapTapGesture = new UITapGestureRecognizer(ShowOptions) { Enabled = true };
            BottomView.AddGestureRecognizer(tapTapGesture);
        }



        partial void BtnEdit_Clicked(Foundation.NSObject sender)
        {
            
        }

        #endregion

        void ShowMessage(string title, string message, UIViewController fromViewController)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
            fromViewController.PresentViewController(alert, true, null);
        }
        #region UIImagePickerControllerDelegate

        [ExportAttribute("imagePickerControllerDidCancel:")]
        public void Canceled(UIImagePickerController picker)
        {
            picker.DismissViewController(true,null);
        }

        [Export("imagePickerController:didFinishPickingMediaWithInfo:")]
        public void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            var imge = info[UIImagePickerController.OriginalImage] as UIImage;
            ImgProfile.Image = imge;
            picker.DismissViewController(true, null);
        }

        #endregion

    }
}
