using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Budget.Model
{
    class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FirstRow { get; set; }
        public DataTemplate MovementsRow { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var listView = container as ListView;
            var index = listView.TemplatedItems.GetGlobalIndexOfItem(item);

            if (index == 0 || index == listView.TemplatedItems.Count - 1)
            {
                return FirstRow;
            } else
            {
                return MovementsRow;
            }
        }
    }
}
