// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PhotoPicker09
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIView BottomView { get; set; }

        [Outlet]
        UIKit.UIImageView ImgBottom { get; set; }

        [Outlet]
        UIKit.UIImageView ImgProfile { get; set; }

        [Outlet]
        UIKit.UILabel LblEdit { get; set; }

        [Outlet]
        UIKit.UILabel LblTap { get; set; }

        [Outlet]
        UIKit.UIView ProfileView { get; set; }

        [Action ("BtnEdit_Clicked:")]
        partial void BtnEdit_Clicked (Foundation.NSObject sender);
        
        void ReleaseDesignerOutlets ()
        {
            if (ProfileView != null) {
                ProfileView.Dispose ();
                ProfileView = null;
            }

            if (ImgProfile != null) {
                ImgProfile.Dispose ();
                ImgProfile = null;
            }

            if (LblEdit != null) {
                LblEdit.Dispose ();
                LblEdit = null;
            }

            if (BottomView != null) {
                BottomView.Dispose ();
                BottomView = null;
            }

            if (ImgBottom != null) {
                ImgBottom.Dispose ();
                ImgBottom = null;
            }

            if (LblTap != null) {
                LblTap.Dispose ();
                LblTap = null;
            }
        }
    }
}
