   M S B A M L   `   `   `  ����M  IWpfApp_BarleyBreak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WpfApp_BarleyBreakWpfApp_BarleyBreak  5   	   #    WpfApp_BarleyBreak.MessageBox   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WpfApp_BarleyBreak     5   	     � Title$  Message Box��5   	   $	��250��6   $	��300��6*    � SizeToContent$ WidthAndHeight=�66      BaseLinearGradientWindow#���   5	   	    � AllowsTransparency .  � WindowStyle$
 None=�6V    � WindowStartupLocation$ CenterOwner=�6i    � 	Resources 5       �� 
TargetType �� Property  
btTemplate �� Value��%R   '��       7  �� 5   
   " ��6   .�5      ��  � ��8   6)   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5        $��MessageContainer��5   L   $��Center=�6   $��Center=�61   $��WrapWithOverflow=�6d   $��10q�6�   �� 5   
   -   5        $��ButtonContainer��5      $��1��6,   
 �� Orientation$
 
Horizontal=�69   $��Right=�5      $��10q�61    5      ��%�   '��       7  �� 5      " ��6   .�5      ��  � ��8   61   �� 5       � ��$ 25��62   �� 5       � ��$ 75��61   �� 5       � ��$ 10,0,0,0q�6/   5      5       5!      5"      5#      