﻿#pragma checksum "C:\Users\vubao\Desktop\Semester_4\.net c#\Final\ver2\BaoNamVu_SpringFinalProject\LoginPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4998EACA8450F5AC4A5127532EDA91DEC49A2FDE1C9354EC10CC2050445B04DD"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BaoNamVu_SpringFinalProject
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // LoginPage.xaml line 34
                {
                    this.registeredUserNameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 3: // LoginPage.xaml line 35
                {
                    this.registeredPasswordTextBox = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 4: // LoginPage.xaml line 36
                {
                    this.registeredFullNameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // LoginPage.xaml line 37
                {
                    this.registeredAddressTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // LoginPage.xaml line 38
                {
                    this.registeredPostalCodeTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 7: // LoginPage.xaml line 39
                {
                    this.registerButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.registerButton).Click += this.registerButton_Click;
                }
                break;
            case 8: // LoginPage.xaml line 22
                {
                    this.userNameTextBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9: // LoginPage.xaml line 23
                {
                    this.passwordTextBox = (global::Windows.UI.Xaml.Controls.PasswordBox)(target);
                }
                break;
            case 10: // LoginPage.xaml line 24
                {
                    this.loginButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.loginButton).Click += this.loginButton_Click;
                }
                break;
            case 11: // LoginPage.xaml line 26
                {
                    this.loginAsGuestButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.loginAsGuestButton).Click += this.loginAsGuestButton_Click;
                }
                break;
            case 12: // LoginPage.xaml line 28
                {
                    this.loginAsAdminButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.loginAsAdminButton).Click += this.loginAsAdmin_Click;
                }
                break;
            case 13: // LoginPage.xaml line 29
                {
                    this.resultTextBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
