   M S B A M L   `   `   `  ����G  CWpfApp_Minor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WpfApp_MinorWpfApp_Minor  5   	       WpfApp_Minor.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WpfApp_Minor     5   	   -   5          �Name$  mainWindow_Minor��5   	      BaseLinearGradientWindow#���   5	   	    � WindowStyle$
 None=�5
   	   $��Segoe Script)�6   $��14(�66    � AllowsTransparency .  � SizeToContent$ WidthAndHeight=�5   	    � Title$ Minor��5   	   $	��300��6   $	��300��6'   '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5   	      ResizeBorderThickness$ 5q�5   	    � 	Resources 5      	 �� 
TargetType
 �� Property  chbTemplate �� Value��%R   '��       7  �� 5   
   "	 ��6   .�5      �� 
 � ��8   6)   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $��* �6   6+    � RowDefinitions 5!   
   �� 5"      $
��Auto �6   $��20��6*   6<   �� 5#      $
��Auto �6   $��20��6*   6<   �� 5$      $��* �6   6)   .+�5&   
   �� -   5         ��Name$ button_NewGame��5&   8     
btTemplate#��[ 6   $��0��6P   $��0��6`   $	��150��6m   $��75��6|   $��25��6�   $��25��6�   $��50��6�   $��3q�6�   $��Center=�6�   $��Center=�6�   .��6  
New Game6   �� 5'   
   $��1��6   .+�5(      �� -   5         ��Name$ checkBox_SoundOnOff��5(      $��Sound ON/OFF��64   
������  d� 	IsChecked$
 TrueY�6�   �� 5)      -   5        $ checkBox_MusicOnOff��5)      $��Music ON/OFF��64   
������ $
 TrueY�6�   5*      �� 5+   
   -   5         ��Name$ textBlock_Time��5+      $��0��6,   $��2��6<   $��1��6P   $��Center=�6]   $�� ��6z   �� 5,   
   -   5         ��Name$ 
borderGame��5,      $��1q�6%   
�������$��10q�5-      $��0��6   $��2��6-   $��2��6A   }�. ��5.      ��  �� ElementName$ uniformGrid_Place�� �� Path$ Width� �� Mode$ TwoWay=���5/      �� $ uniformGrid_Place��$ Height�$ TwoWay=�._�51      A� -   5         A�Name$ uniformGrid_Place��51      $��2��52      $��2��6'   }�. $��Disabled=�54      55      56      57      