namespace RMExtractor {
    partial class frmMain {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent () {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new DarkUI.Controls.DarkLabel();
            this.txtProjectPath = new DarkUI.Controls.DarkTextBox();
            this.btnExplore = new DarkUI.Controls.DarkButton();
            this.btnUnpack = new DarkUI.Controls.DarkButton();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.btnPack = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Projeto (XP/VX/VXA):";
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.txtProjectPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProjectPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtProjectPath.Location = new System.Drawing.Point(12, 27);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(269, 20);
            this.txtProjectPath.TabIndex = 1;
            // 
            // btnExplore
            // 
            this.btnExplore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExplore.Location = new System.Drawing.Point(290, 27);
            this.btnExplore.Name = "btnExplore";
            this.btnExplore.Padding = new System.Windows.Forms.Padding(5);
            this.btnExplore.Size = new System.Drawing.Size(31, 20);
            this.btnExplore.TabIndex = 2;
            this.btnExplore.Text = "...";
            this.btnExplore.Click += new System.EventHandler(this.btnExplore_Click);
            // 
            // btnUnpack
            // 
            this.btnUnpack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnpack.Location = new System.Drawing.Point(220, 53);
            this.btnUnpack.Name = "btnUnpack";
            this.btnUnpack.Padding = new System.Windows.Forms.Padding(5);
            this.btnUnpack.Size = new System.Drawing.Size(101, 23);
            this.btnUnpack.TabIndex = 6;
            this.btnUnpack.Text = "Exportar";
            this.btnUnpack.Click += new System.EventHandler(this.btnUnpack_Click);
            // 
            // btnPack
            // 
            this.btnPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPack.Location = new System.Drawing.Point(113, 53);
            this.btnPack.Name = "btnPack";
            this.btnPack.Padding = new System.Windows.Forms.Padding(5);
            this.btnPack.Size = new System.Drawing.Size(101, 23);
            this.btnPack.TabIndex = 7;
            this.btnPack.Text = "Importar";
            this.btnPack.Click += new System.EventHandler(this.btnPack_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 87);
            this.Controls.Add(this.btnPack);
            this.Controls.Add(this.btnUnpack);
            this.Controls.Add(this.btnExplore);
            this.Controls.Add(this.txtProjectPath);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Package Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkLabel label1;
        private DarkUI.Controls.DarkTextBox txtProjectPath;
        private DarkUI.Controls.DarkButton btnExplore;
        private DarkUI.Controls.DarkButton btnUnpack;
        private System.Windows.Forms.OpenFileDialog ofd;
        private DarkUI.Controls.DarkButton btnPack;
    }
}

