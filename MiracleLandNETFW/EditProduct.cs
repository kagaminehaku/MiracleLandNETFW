﻿using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiracleLandNETFW
{
    public partial class EditProduct : Form
    {
        private int eid;
        public EditProduct(int id)
        {
            eid = id;
            InitializeComponent();
            LoadProductToEdit();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            string imagePath = pictureBox1.ImageLocation;
            if (imagePath == null || imagePath.Length > 1024)
            {
                MessageBox.Show("Image path invaild.");
                return;
            }
            else if (String.IsNullOrEmpty(edit_name.Text) || edit_price == null || edit_quantity == null || String.IsNullOrEmpty(edit_info.Text))
            {
                MessageBox.Show("Please fill all field.");
                return;
            }
            var busproduct = new BUSproduct();
            if (int.TryParse(edit_price.Text, out int price) && int.TryParse(edit_quantity.Text, out int quantity))
            {
                bool editstate = busproduct.EditProduct(eid,edit_name.Text, price, quantity, edit_info.Text, imagePath);
                if (editstate)
                {
                    MessageBox.Show($"{edit_name.Text}" + "Edit", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else if (editstate == false)
                {
                    MessageBox.Show("An Unexpected Error Occurred.");
                    return;
                }
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadProductToEdit()
        {
            var busproduct = new BUSproduct();
            var product = busproduct.GetProduct(eid);
            edit_name.Text = product.pname;
            edit_price.Text = product.pprice.ToString();
            edit_quantity.Text = product.pquantity.ToString();
            edit_info.Text = product.pinfo;
            pictureBox1.ImageLocation = product.pimg;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            NewImage();
        }

        private void NewImage()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Title = "Select an Image";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    string currentDirectory = Environment.CurrentDirectory;

                    string imagesFolderPath = Path.Combine(currentDirectory, "Images");
                    if (!Directory.Exists(imagesFolderPath))
                    {
                        Directory.CreateDirectory(imagesFolderPath);
                    }

                    string destinationPath = Path.Combine(imagesFolderPath, Path.GetFileName(selectedImagePath));
                    File.Copy(selectedImagePath, destinationPath, true);

                    pictureBox1.ImageLocation = destinationPath;
                }
            }
        }

        private void edit_price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 127)
            {
                e.Handled = true;
            }
        }

        private void edit_quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 127)
            {
                e.Handled = true;
            }
        }
    }
}
                