   M S B A M L   `   `   `  ����E  ACloudNotes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:CloudNotes
CloudNotes  5   	       CloudNotes.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:CloudNotes     5   	   -   5          �Name$  MainWindow1��5   	    � Title$ 
CloudNotes��5   	   $	��300��5	   	   $	��450��6   $��Consolas)�5
   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ CanMinimize=�6.   .��5      � -   5         �Name$ MainGrid��5       � RowDefinitions 5   
   �� 5      $��0.65* �6   �� 5      $��0.20* �6    � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      $��0.25* �6   .+�5   
   �� -   5         ��Name$ DropBoxBorder��5      $��3q�6(   $��3��63   $��2q�5      ���	#FF116ED6._�5   
   �� -   5         ��Name$ DropBox��5      ���	#C11F1F1F$��0��5      $��12(�5      $
��Bold"�6!   	 �� TextAlignment$	 Center=�5      
 �� Padding$
 0 80 0 0q�6*    J� 	AllowDrop$
 True��5   &   ���	#FF68A6EC5    M   5!      �� 5$   
   -   5         ��Name$ OpenSettingsButton��5$      $��45��5%      $��45��6   $��1��5&      $��2��6   $��3q�5'      $��2q�6   $��Center=�5(      $��Center=�6.   .��5)      �� $%��Images\settings_button_icon.png��6   5*      �� 5,   
   -   5        $ DownloadFilesButton��5,      $5��/Загрузить файлы из облака��6.   $��45��5-      $	��190��6   $��12(�5.      $��0q�6   $��1��5/      $��1��6   $��3q�50      $��Center=�51      $��Center=�52      6,   �� 54   
   -   5        $ UploadToCloudButton��54      $3��-Загрузить файлы в облако��6.   $��45��55      $	��190��6   $��12(�56      $��0q�6   $��1��57      $��3q�6   $��Center=�58      $��Center=�59      6,   5;      5<      