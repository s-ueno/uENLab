using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uEN.UI;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(Vol09View))]
    public class Vol09ViewModel : BizViewModel
    {
        public override string Description { get { return "メッセージ　ボックス"; } }

        /// <summary>デフォルトコンストラクタ</summary>
        public Vol09ViewModel()
        {

        }

        public void OkAction()
        {
            this.ShowOk("メッセージのデザイン",
                "ユーザーが操作を中断して、見たり認識したりする必要がある緊急の情報を伝えるには、メッセージ ダイアログを使います。",
                OkActionInternal);
        }
        private void OkActionInternal()
        {
            StatusMessage = "OK を押しました";
        }



        public void YesNoAction()
        {
            this.ShowYesNo("メッセージのデザイン",
                "ユーザーの入力を必要とするブロック質問を表示するには、メッセージ ダイアログを使います。",
                YesAction,
                () => StatusMessage = "いいえ を押しました");
        }
        private void YesAction()
        {
            StatusMessage = "はい を押しました";
        }



        public void YesNoCancelAction()
        {
            this.ShowYesNoCancel("メッセージのデザイン",
                "ユーザーの明示的な操作を求める場合、またはユーザーが認識することが重要であるメッセージを表示する場合は、メッセージ ダイアログを使います",
                () => StatusMessage = "はい を押しました",
                () => StatusMessage = "いいえ を押しました",
                null);
        }





        public void CustomAction()
        {
            this.ShowMessage("メッセージのデザイン",
                "すべてのダイアログでは、(タイトルの有無に関係なく) ダイアログ内のテキストの 1 行目で、ユーザーの目的 (実行する内容) を明確に示す必要があります。",
                new MessageDialogHelper.Command("はい、そうです", () => { }, true),
                new MessageDialogHelper.Command("いいえ、違います", () => { }));
        }
    }
}
