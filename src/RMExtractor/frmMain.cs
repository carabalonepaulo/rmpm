using DarkUI.Forms;

namespace RMExtractor {
    public partial class frmMain : DarkForm {
        public frmMain () {
            InitializeComponent ();

            ofd.Filter = "RMXP (*.rxproj)|*.rxproj|RMVX (*.rvproj)|*.rvproj|RMVXAce (*.rvproj2)|*.rvproj2";
            ofd.CheckFileExists = true;
        }
                
        private void btnExplore_Click (object sender, System.EventArgs e) {
            ofd.ShowDialog ();
            txtProjectPath.Text = ofd.FileName;
        }

        private void btnUnpack_Click (object sender, System.EventArgs e) {
            if (string.IsNullOrEmpty (txtProjectPath.Text)) {
                DarkMessageBox.ShowWarning ("Preencha todos os campos antes de continuar.", "Aviso");
                return;
            }

            Project project = new Project (txtProjectPath.Text);
            project.Unpack ();
        }

        private void btnPack_Click (object sender, System.EventArgs e) {
            if (string.IsNullOrEmpty (txtProjectPath.Text)) {
                DarkMessageBox.ShowWarning ("Preencha todos os campos antes de continuar.", "Aviso");
                return;
            }

            Project project = new Project (txtProjectPath.Text);
            project.Pack ();
        }
    }
}
