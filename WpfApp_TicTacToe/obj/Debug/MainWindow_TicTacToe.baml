   M S B A M L   `   `   `  ����K  GWpfApp_TicTacToe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:WpfApp_TicTacToeWpfApp_TicTacToe  5   	   +    %WpfApp_TicTacToe.MainWindow_TicTacToe   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:WpfApp_TicTacToe     5   	   -   5          �Name$  mainWindowTicTacToe��5	   	      BaseLinearGradientWindow#���   5   
    � Title$ 	TicTacToe��5
   	   $��Segoe Script)�5   	   $��14(�6#   $	��600��5   	   $	��400��6   $	��400��6"   $	��600��61    � SizeToContent$ WidthAndHeight=�6A    � AllowsTransparency .  � WindowStyle$
 None=�5   #   '  !System.Windows.Shell.WindowChrome   WindowChrome 5        5   
      CaptionHeight$ 0Q�5   	      ResizeBorderThickness$ 5q�5   	    � 	Resources 5        ControlLightColor  ControlMediumColor  ControlDarkColor  MenuPopupBrush	 �� EndPoint
 �� 
StartPoint � Color � Offset $  ProgressBarIndicatorAnimatedFill��%�  &	       &	      &	 >     &	 _     &	 &    ��5   
   	White��61   ��5   
   	#FF7381F9��66   ��5   
   	#FF211AA9��64   �� 5   
   $	 0.5,1&�5      $
 0.5,0&�5      .��5      � # �  6   $ 0Q�5       � 5!      # �  6   $	 0.5Q�5"      � 5#      # �  6   $ 1Q�5$      5%      �� 5'   
   $	
 0,0&�5(      $		 1,0&�5)      ��5+      � � 5,      $ 	#000000FF��6#   $ 0Q�5-      � 5.      $ 	#600000FF��6#   $	 0.4Q�5/      � 50      $ 	#600000FF��6#   $	 0.6Q�51      � 52      $ 	#000000FF��6#   $ 1Q�53      54      56      5=      .��5?      ]� .+�5@   
   o� $
��Auto��6   $	��Top=�6   
������ ��5A      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType" � �� Path$ 
FontFamily���5B      ��  �� *FindAncestor+" �$ FontSize���5C      �� 5D      .R�5E      ]� $��Stretch=�6    5F      5H      .@�5J      m� -   5         m�Name$ menuItem_NewGame��5J      $��New Game��61     menuItemTemplate#��[ 6C   6p   �� 5K      m� 5L      -   5        $ menuItem_SoundOnOff��5L      $��Sound ON/OFF��64   #��[ 6J   6w   m� 5M      -   5        $ menuItem_MusicOnOff��5M      $��Music ON/OFF��64   #��[ 6J   6w   �� 5N      m� 5O      -   5        $ menuItem_Pause��5O      $��Pause��6/   #��[ 6>   6k   m� 5P      -   5        $��menuItem_Exit��5P      $
��Exit��6,   $��Right=�6:   #��[ 6V   6�   5R      �� 5S   
   $	��Top=�6   $��Center=�6*    �� Orientation$ 
Horizontal=�6G   .+�5T      �� $��Center=�6   .+�5U      �� $��Center=�6   .��65   
Computer6?   �� 5V      -   5         ��Name$ textBlock_Computer��5V      $��3q�68   $��Center=�6C   06c   5W      �� 5X      $��Center=�6   .+�5Y      �� $��Center=�6   .��65   :68   5Z      �� 5[      $��Center=�6   .+�5\      �� $��Center=�6   .��65   Player6=   �� 5]      -   5        $ textBlock_Player��5]      $��3q�66   $��Center=�6A   06a   5^      5_      �� 5`   
   $	��Top=�6   $ 
Horizontal=�6*   $��Right=�6C   .+�5a      �� .��6   Time:6   �� 5b      -	   5        $ textBlock_Time��5b      $	��100��60   $��3q�6<   $��Center=�6G   6f   5c      � 5d   
   .+�5e      �� -
   5         ��Name$ 
borderGame��5e      $��1q�6)   
�������$��10q�5f      $��1��6   }�.  �� CornerRadius$ 20��5g      ��5h      ��  �� ElementName$ uniformGridGame��$ Width� �� Mode$ TwoWay=���5i      �� $ uniformGridGame��$ Height�$ TwoWay=�._�5l      A� -   5        $��uniformGridGame��5m       A� Columns$ 3��5n       A� Rows$ 3��6   }�. $��Disabled=�5p      5r      5s      �� 5t      -   5        $��labelGameEnd��5t   )    K� ZIndex$
 1000��6   
������ ._�5v      � .+�5w      �� $��45(�6   $��Center=�6*   $��Center=�6E   
���  ��.��6s   Congratulation!!!6�   5x      5y      �� 5z      -   5        $��
labelPause��5z   )   $
 1000��6   
������ $y�Hidden=�5{      ._�5|      � .+�5}      �� $��45(�6   $��Center=�6*   $��Center=�6E   
���  ��.��6s   PAUSE6z   5~      5      5�      5�      5�      