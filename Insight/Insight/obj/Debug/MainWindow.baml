   M S B A M L   `   `   `  ����B  >Insight, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:InsightInsight  5   	   -clr-namespace:Insight.ViewsInsight.Views  5   	       Insight.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Insight     5   	   .customclr-namespace:Insight.Views     5   	     � Title$  Insight��5	   	   $	��799��6   $
��1200��6&    � 
ResizeMode$ NoResize=�63    � WindowState$ 	Maximized=�6I    � WindowStyle$
 None=�6a      PrimaryBrush#���   6t   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��0 �6   �� 5      $��* �6   �� 5      $	��400 �6   .+�5   
   � $��1��6   #���   6    � RowDefinitions 5      �� 5      $��* �6    �� 5      $	��175 �6    �� 5      $	��175 �6    �� 5      $	��175 �6    �� 5      $��* �6     5      �� 5      $��* �6#   �� 5      $	��175 �6#   �� 5      $	��175 �6#   �� 5      $	��175 �6#   �� 5      $	��175 �6#   �� 5       $��* �6#   .+�5#         Insight.Views.MenuButton  -   5          Name$ cmdNewSetup��5#   =   $��1��6    $��1��60   6T     5$      -   5        $ cmdViewSetups��5$   =   $��2��6    $��1��60   6V     5%      -   5        $ cmdStats��5%   =   $��3��6    $��1��60   6Q     5&      -   5        $ cmdCompetitions��5&   =      Insight.Views.MenuButton   	MouseDown$ ViewCompetitions��6V   $��4��6    $��1��60   6u     5'      -   5        $ cmdBets��5'   =   $��1��6    $��2��60   6P   5*      � 5-   
   $��2��6   ���#ffffff 5/      �� 50      $
��Auto �6    �� 51      $
��Auto �6    �� 52      .+�55      �� -   5         ��Name$ imgCompetitionIcon��55      $
��Auto��60   $��60��6=   $��10, 10, 10, 0q�6I   �� 57      ���	#07000000$��2��62   $
��Auto=�6?   $��Disabled=�6b   $��Stretch=�6�   $��Stretch=�6�   .��58      �� -   5        	 ��Name$	 	lstLeague��58      59      � 5;      $��1��6    5<      �� 5=      $��30 �6'   �� 5>      �� 5?      $��30 �6'   .+�5B      �� -   5        
 ��Name$
 cmdLeftChevron��5B      $��0��6I   $��3q�6Y   $
��Wrap=�6d   $��<��6x   $��Center=�6�   $��Center=�6�   $��22(�6�     	TextBrush#���  6�   �� 5C      -	   5        $
 txtCompetitionName��5C      $��1��68   $��3q�6H   $
��Wrap=�6S   $��Championship��6g   $��Center=�6{   $��Center=�6�   $��22(�6�   #���  6�   $��Cambria)�6�   �� 5D      -
   5        $
 cmdRightChevron��5D      $��2��6K   $��3q�6[   $
��Wrap=�6f   $��>��6z   $��Center=�6�   $��Center=�6�   $��22(�6�   #���  6�   5F      5H      5I      5J      