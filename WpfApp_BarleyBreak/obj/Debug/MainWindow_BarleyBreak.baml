   M S B A M L   `   `   `  ����M  IWpfApp_BarleyBreak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WpfApp_BarleyBreakWpfApp_BarleyBreak  5   	   /    )WpfApp_BarleyBreak.MainWindow_BarleyBreak   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WpfApp_BarleyBreak     5   	   -   5          �Name$  mainWindowBarleyBreak��5   	    � Title$ Barley Break��5   	   $	��600��5	   	   $	��550��6   $	��450��5
   	   $	��450��6      BaseLinearGradientWindow#���   5   	    � WindowStyle$
 None=�5   	   $��Segoe Script)�6   $��14(�66    � AllowsTransparency . '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5   	      ResizeBorderThickness$ 5q�5   	    � 	Resources 5       �� 
TargetType	 �� Property  
btTemplate
 �� Value  chbTemplate��%�   '��       7  '�� D     7  �� 5   
   " ��6   .�5      �� 	 � ��8
   6)   5       �� 5"   
   " ��6   .�5#      �� 	 � ��8
   6)   5$      5(      .��5*      �  � ColumnDefinitions 5+   
   �� 5,      $��* �6   6+    � RowDefinitions 5.   
   �� 5/      $
��Auto �6   6,   �� 50      6   .+�52   
   �� .+�53      ��  �� Orientation$ 
Horizontal=�6   .+�54      �� -   5        $	��100��54      $��10,3,3,3q�6%   .��6S   
New Game6]   �� 55      $��Center=�6   $ 
Horizontal=�6:   .+�56      �� .+�57      �� $��Center=�6    .��6D   Rows6J   �� 58      -   5         ��Name$ textBox_Rows��5;   (   $��0q�59   "   $��Center=�5:      $��Center=�6>   $��18(�5;      $��50��6>   
������ ��58   "   �� *	base_rows+5;   b   5<      �� 5=      $��Bottom=�6!   .+�5>      �� $��Center=�6    .��6D   x6G   5?      �� 5@      .+�5A      �� $��Center=�6    .��6D   	Columns6M   �� 5B      -   5        $ textBox_Columns��5E   (   $��0q�5C   "   $��Center=�5D      $��Center=�6>   $��18(�5E      $��50��6A   
������ ��5B   "   �� *base_columns+5E   e   5F      5G      �� 5H      -   5         ��Name$ checkBoxPicture��5H   4   
������ $	��100��6M   $��3q�6Y   $��Center=�6d   .��6�   Use Picture6�   �� 5I      -   5        $	��100��5I      $��3q�6%   .��6L   Safe Shuffle6Z   �� 5J      $��Center=�6   .��6:   Count:6B   �� 5K      -   5        $��textBoxCountShuffle��5K   q   
������ $��Center=�63   $��0,0,0,1q�6W   $��40��6�   $��3q�6�   $��Center=�6�   .��6�   106�   5L      � 5M       5N      �� 5O      $
��Auto �6'   66   �� 5P      $��* �6'   63   .+�5R      � $��0��6    5S      �� 5T      $
��Auto �6+   6:   �� 5U      $��* �6+   67   .+�5W      �� $��0��6   .��6,   	Player:65   �� 5X      -   5        $��1��5X      
 � Tag$ Enter Player name...��5Y        WatermarkTextBox#��[ 5Z      ��5X   .   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" � �� Path$ 
PlayerName� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5[   !   5\      �� 5]      $��1��6   $ 
Horizontal=�6-   $��Right=�6F   .+�5^      �� .��6   Time:6#   �� 5_      -	   5         ��Name$ textBlock_Time��5_       $	��100��68   $��3q�6D   $��Center=�6O   6n   5`      5a      �� 5b      $��1��6   $ 
Horizontal=�6)   $��Center=�6B   .+�5c      �� -
   5         ��Name$ buttonSaveGame��5c      $	��100��61   $��3q�6=   .��6f   	Save Game6q   �� 5d      -   5        $ buttonLoadGame��5d      $	��100��61   $��3q�6=   .��6f   	Load Game6q   5e      5f      �� 5g   
   -   5         ��Name$ 
borderGame��5g      $��1q�6%   
�������$��10q�5h      $��1��6   }�. ._�5k      A� -   5        $��uniformGridGame��5l      $��1��5m      }�. $��Disabled=�5p       A� Columns 5n      �� *base_columns+ A� Rows 62   �� *	base_rows+5s      5t      �� 5u   
   -   5        $��labelGameEnd��5u   %    K� ZIndex$
 1000��6   
������ $��0��5v      $��2��6   ._�5x      � .+�5z      �� $��45(�6   $��Center=�6&   $��Center=�6A   
���  ��.��6o   Congratulation!!!6�   �� 5�       �� CornerRadius$ 	20,0,20,0��6     linearGradientBrush#��[ 62   $��2q�6d   
����� �$	��200��5�      ��6%   ��  �� ElementName$ stackPanelMenu��$ ActualHeight�._�5�      �� -   5         ��Name$ stackPanelMenu��5�   !   
������ $
��Auto��6R   .+�5�      �� $��3q�6    $��Center=�6+   $��Center=�6H   $��20(�6c   $
��Bold"�6q   .��6�   MENU6�   �� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��New Game��6`   ��6u   �� *NewGameCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��	Load Game��6`   ��6u   �� *LoadGameCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      6"   �� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��	Set Image��6`   ��6u   �� *SetImageCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��Sound On/Off��6`   ��6w   �� *SoundOnOffCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��Music On/Off��6`   ��6w   �� *MusicOnOffCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��
High Score��6`   ��6u   �� *HighScoreShowCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      6"   �� 5�      #��[ 6!   $	��150��6H   $��30��6T   $
��Exit��6`   ��6u   �� *ExitCommand+��6�   �� $ DataContext� �� *Self+�� 5�      6"   5�      5�      5�      5�      5�      5�      