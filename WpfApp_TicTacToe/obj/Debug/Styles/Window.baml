   M S B A M L   `   `   `  ����K  GWpfApp_TicTacToe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:WpfApp_TicTacToe.StylesWpfApp_TicTacToe.Styles  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35   buttonControlRoundTemplate  �� 
TargetType A�Name  linearGradientBrush J� RenderTransformOrigin 8� X 8� Y �� ScaleX �� ScaleY �� Triggers P� Property	 P� Value
 �� Property �� Value3   -System.Windows.Media.Effects.DropShadowEffect    Opacity    
BlurRadius    Color  WindowControlTemplate �� CornerRadius � RowDefinitions ��Name.  (System.Windows.Interactivity.Interaction   Triggers/  )System.Windows.Interactivity.EventTrigger   	EventName.  (System.Windows.Interactivity.TriggerBase   Actions6  0System.Windows.Interactivity.InvokeCommandAction   Command   CommandParameter �� RelativeSource �� AncestorType �� Orientation ]� 
Conditions �� Property �� Value �� 
TargetName �� EndPoint �� 
StartPoint  � Color! � Offset  BaseLinearGradientWindow" �� 
TargetType# �� Triggers$ :� RoutedEvent% �� TargetProperty	& �� To' g� RepeatBehavior( g� AutoReverse) g� Duration	* ,� To�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      /local%clr-namespace:WpfApp_TicTacToe.Styles  5      Ninteractivity:http://schemas.microsoft.com/expression/2010/interactivity  5      %�  &	        &	 �    &	 �	    &	 .
    7  �� 5"      "  ��69   .R�5#   
   � .+�5$      A� $ outerCircle��6   #��z��5%      #��z��5&      
������ $��2��5(      #���  6*    5*      8� 5+      $	 0.5Q�6   $	 0.5Q�6$   z�5.      �� 5/      $	 0.8Q�6%   $	 0.8Q�62   51      �� 53      #��z��54      #��z��55      57       59   
   P� 5;       �@J�IsMouseOver	 . . �5<      �� 
 �@J�Effect 5=         5>      shadowEffect��$	 1.0Q�6A   $ 50Q�6O   $
 Gold��6_   5@      5A      P� 5C       � ��	 . . �5D      �� 
 � z� 5E      �� 5F      $ .9Q�6)   $ .9Q�65   5H      �� 5I      
 �@J�RenderTransformOrigin$ .5,.5&�6:   5J      5N      �� 5P      "  �64   .R�5Q   
   �� $ 20��5R      #��z��5S   	   #��z��5T   	   #��z��5U   	   ._�5W      �  5X      �� 5Y      $
��Auto �6$   �� 5Z      $��0.93* �6$   �� 5[      $
��Auto �6$   .+�5^      �� $��1��5_      #��z��5`      #��z��5a      �� 5c      $ WindowResizeGrip��5e      $��Right=�5d      $��Bottom=�5f      ��.  $y�	Collapsed=�5h      $��2��5i      �� 5k      $	��Top=�6   $
��Auto��6.   $
��Auto��6;   ._�5l      ]� $	��Top=�6    
������  5n        5o      $ 	MouseDown��69   . 5p   "      6D   �� *DragCommand+ 6d   ��  �� *FindAncestor+$ WindowL�5q      .+�5t      �� $
��Left=�6{   $
��Left=�6�   ��6    �� *Title+ �� *FindAncestor+$ WindowL�6�   �� 5u      $ 
Horizontal=�6%   $��Right=�6>   $��Center=�6Z   $��30��6u   $��Right=�6�   .+�5v      �� #���   6%   $��20��6]   $��20��6h   $��5q�6t   $	��Top=�6�   ��6   �� *HideCommand+��6�   ��  �� *FindAncestor+$ WindowL�.��6  _6  �� 5w      #���   6%   $��20��6]   $��20��6h   $��5q�6t   ��6   �� *MinimizedCommand+��6�   ��  �� *FindAncestor+$ WindowL�.��6  口6  �� 5x      #���   6%   $��20��6]   $��20��6h   $��5q�6t   ��6   �� *CloseCommand+��6�   ��  �� *FindAncestor+$ WindowL�.��6�   X6  5y      5z      5{      5}      5~       5�   
   ]� 5�       5�      �� 5�       �@�
ResizeMode$ CanResizeWithGrip=�66   �� 5�       �@�WindowState$ Normal=�67   ./�5�      �� $ WindowResizeGrip��6/   
 � y�$ Visible=�6M   5�      5�      �� 5�      $ 0.2,1.0&�66   $ 0.8,0.0&�6I   .��5�   
   � $
  Aqua��6   $	! 0.8Q�6$   � 5�   
   $  Magenta��6   $	! 0.2Q�6'   5�      �� 5�      BaseLinearGradient��"" �6I   .�5�   
   �� 
 � ��8   6%   �� 5�   
   
 � ��# �  6'   # 5�   
   :� 5�      $$ Loaded��6   [�5�      �� 5�      .��5�      �� .��5�      �� $F% @Background.(GradientBrush.GradientStops)[0].(GradientStop.Color)�6-   $& MagentaX�6�   $' Forever��6�   ( . $) 0:0:10D�6�   �� 5�      $F% @Background.(GradientBrush.GradientStops)[1].(GradientStop.Color)�6-   $
& AquaX�6�   $' Forever��6�   ( . $) 0:0:10D�6�   ,� 5�      $1% +Background.(LinearGradientBrush.StartPoint)�6-   $* 1.0 0.0X�6u   $' Forever��6�   ( . $) 00:00:3D�6�   ,� 5�      $/% )Background.(LinearGradientBrush.EndPoint)�6-   $* 0.0 1.0X�6s   $' Forever��6�   ( . $) 00:00:3D�6�   5�      5�      5�      5�      5�      