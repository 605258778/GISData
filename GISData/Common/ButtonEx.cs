using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
public class ButtonEx : Button
{
    public new event EventHandler DoubleClick;
    DateTime clickTime;
    bool isClicked = false;
    object iTag;
    string iName;
    string iText;
    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (isClicked)
        {
            TimeSpan span = DateTime.Now - clickTime;
            if (span.Milliseconds < SystemInformation.DoubleClickTime)
            {
                DoubleClick(this,e);
                isClicked = false;
                
            }
            else
            {
                isClicked = true;
                clickTime = DateTime.Now;
            }
        }
        else
        {
            isClicked = true;
            clickTime = DateTime.Now;
        }
        iName = this.Name;
        iText = this.Text;
        iTag = this.Tag;
    }
}