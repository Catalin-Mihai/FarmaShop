namespace FarmaShop.Web.ViewModels
{
    public class ConfirmationDialogViewModel
    {
        public string DialogMessage { set; get; }
        public string DismissButton { set; get; }
        public string ConfirmButton { set; get; }
        
        public string AjaxMethod { set; get; }
        public string AjaxUrl { set; get; }

        public string RedirectLocation { set; get; }
        
        public string[] Parameters { get; set; }
        
        public string SuccesMessage { set; get; }
        
        public string FailMessage { set; get; }
    }
}