   M S B A M L   `   `   `  ����H  DWpfApp_Puzzle, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfApp_PuzzleWpfApp_Puzzle  5   	   %    WpfApp_Puzzle.MainWindow_Puzzle   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WpfApp_Puzzle     5   	   -   5          �Name$  mainWindowPuzzle��5   	    � Title$ Puzzle��5   	   $	��600��5	   	   $	��550��6   $	��450��5
   	   $	��900��6      BaseLinearGradientWindow#���   5   	    � WindowStyle$
 None=�5   	   $��Segoe Script)�6   $��14(�66    � AllowsTransparency . '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5   	      ResizeBorderThickness$ 5q�5   	    � 	Resources 5       �� 
TargetType	 �� Property  
btTemplate
 �� Value  chbTemplate  imageScaleTransformStyle �� ScaleX �� Path �� UpdateSourceTrigger �� ScaleY  canvasScaleTransformStyle��%8  '��       7  '�� D     7  &	 �     &	 H    �� 5   
   " ��6   .�5      �� 	 � ��8
   6)   5       �� 5"   
   " ��6   .�5#      �� 	 � ��8
   6)   5$      �� 5'   
   " ��61   .�5(      �� 	 � z�
 5)      �� 5*       6%   �� $ ScaleX�$ PropertyChanged=� 6i   �� $ ScaleY�$ PropertyChanged=�5,      5-      �� 5.   
   " ��62   .�5/      �� 	 � z�
 50      �� 51       6%   �� $ ScaleX�$ PropertyChanged=� 6i   �� $ ScaleY�$ PropertyChanged=�53      54      56      .��58      �  � ColumnDefinitions 59   
   �� 5:      $��* �6   6+   �� 5;      $��* �6   6+    � RowDefinitions 5=   
   �� 5>      $
��Auto �6   6,   �� 5?      6   .+�5A   
   �� $��2��6   .+�5B      ��  �� Orientation$ 
Horizontal=�6   .+�5C      �� -   5        $	��100��5C      $��10,3,3,3q�6%   .��6S   
New Game6]   �� 5D      $��Center=�6   $ 
Horizontal=�6:   .+�5E      �� .+�5F      �� $��Center=�6    .��6D   Rows6J   �� 5G      -   5         ��Name$ textBox_Rows��5J   (   $��0q�5H   "   $��Center=�5I      $��Center=�6>   $��18(�5J      $��50��6>   
������ ��5G   "   �� *	base_rows+5J   b   5K      �� 5L      $��Bottom=�6!   .+�5M      �� $��Center=�6    .��6D   x6G   5N      �� 5O      .+�5P      �� $��Center=�6    .��6D   	Columns6M   �� 5Q      -   5        $ textBox_Columns��5T   (   $��0q�5R   "   $��Center=�5S      $��Center=�6>   $��18(�5T      $��50��6A   
������ ��5Q   "   �� *base_columns+5T   e   5U      5V      �� 5W      $	��100��6   �� 5X      -   5        $	��100��5X      $��3q�6%   .��6L   Safe Shuffle6Z   5Y      � 5Z       5[      �� 5\      $
��Auto �6'   66   �� 5]      $��* �6'   63   .+�5_      � $��0��6    5`      �� 5a      $
��Auto �6+   6:   �� 5b      $��* �6+   67   .+�5d      �� $��0��6   .��6,   	Player:65   �� 5e      -   5        $��1��5e      
 � Tag$ Enter Player name...��5f        WatermarkTextBox#��[ 5g      ��5e   .   ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" �$ 
PlayerName� �� Mode$ TwoWay=�$ PropertyChanged=�5h   !   5i      �� 5j      $��1��6   $ 
Horizontal=�6-   $��Right=�6F   .+�5k      �� .��6   Time:6#   �� 5l      -   5         ��Name$ textBlock_Time��5l       $	��100��68   $��3q�6D   $��Center=�6O   6n   5m      5n      �� 5o      $��1��6   $ 
Horizontal=�6)   $��Center=�6B   .+�5p      �� -   5         ��Name$ buttonSaveGame��5p      $	��100��61   $��3q�6=   .��6f   	Save Game6q   �� 5q      -	   5        $ buttonLoadGame��5q      $	��100��61   $��3q�6=   .��6f   	Load Game6q   5r      5s      �� 5t   
   -
   5         ��Name$ 
borderGame��5t      $��1q�6%   
�������$��10q�5u      $��1��6   $��0��6*   }�. ._�5y      �� -   5        $��uniformGridGame��5z      }�. $��Disabled=�5|       J� 	AllowDrop . 5�      5�      �� 5�   
   -   5        $ borderGamePieces��5�      $��1q�6+   
�������$��10q�5�      $��1��6   $��1��6*   }�. ��5�      ��  �� ElementName$ uniformGridGame��$ Width�$ TwoWay=���5�      �� $ uniformGridGame��$ Height�$ TwoWay=�._�5�      �� -   5        $��uniformGridGamePieces��5�      }�. $��Disabled=�5�       . 
������ 5�      5�      �� 5�   
   -   5        $��labelGameEnd��5�   %    K� ZIndex$
 1000��6   
������ $��0��5�      $��2��6   $��2��6+   ._�5�      � .+�5�      �� $��45(�6   $��Center=�6&   $��Center=�6A   
���  ��.��6o   Congratulation!!!6�   �� 5�       �� CornerRadius$ 	20,0,20,0��6     linearGradientBrush#��[ 62   $��2q�6d   
����� �$	��200��5�      ��6%   �� $ stackPanelMenu��$ ActualHeight�._�5�      �� -   5         ��Name$ stackPanelMenu��5�   !   
������ $
��Auto��6R   .+�5�      �� $��3q�6    $��Center=�6+   $��Center=�6H   $��20(�6c   $
��Bold"�6q   .��6�   MENU6�   �� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��New Game��6`   ��6u   �� *NewGameCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��	Load Game��6`   ��6u   �� *LoadGameCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      6"   �� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��Sound On/Off��6`   ��6w   �� *SoundOnOffCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��Music On/Off��6`   ��6w   �� *MusicOnOffCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      #��[ 6!   $	��150��6H   $��30��6T   $��
High Score��6`   ��6u   �� *HighScoreShowCommand+��6�   �� $ DataContext� �� *FindAncestor+$ WindowL��� 5�      6"   �� 5�      #��[ 6!   $	��150��6H   $��30��6T   $
��Exit��6`   ��6u   �� *ExitCommand+��6�   �� $ DataContext� �� *Self+�� 5�      6"   5�      5�      5�      5�      5�      5�      