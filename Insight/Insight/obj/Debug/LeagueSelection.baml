   M S B A M L   `   `   `  ����B  >Insight, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:InsightInsight  5   	       Insight.LeagueSelection   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Insight     5   	     � Title$  Insight��5   	   $	��799��6   $
��1200��6&    � 
ResizeMode$ NoResize=�63    � WindowState$ 	Maximized=�6I    � WindowStyle$
 None=�6a      PrimaryBrush#���   6t   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $	��260 �6   �� 5      $	��260 �6   �� 5      $��2* �6    � ColumnDefinitions 5   
   �� 5      $	��.4* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $	��.4* �6   .+�5   
   � -   5        
 � Tag$ Premiership��5   )   $��1��6;   $��1��6H   ���#ffffff$��Stretch=�6m   $��Stretch=�6�   $��10, 10q�6�   .+�5      �� .+�5      �� -   5         ��Name$ TitlePremiership��5      $��Cambria)�66   $��28(�6K    �� TextAlignment$ Center=�6Y   $��10q�6p   $��Center=�6|   $
��Wrap=�6�   $��Premier League��6�   ���#0f0f0f$	��Top=�6�   �� 5      -   5        $ TeamsPremiership��5      $��Cambria)�66   $��20(�6K   $ Center=�6Y   $��10q�6p   $��Center=�6|   $
��Wrap=�6�   $��20 teams��6�   ���#121212$	��Top=�6�   5       5!      � 5"   
   -   5        $ Championship��5"   )   $��1��6<   $��2��6I   $��Stretch=�6Y   $��Stretch=�6w   $��10, 10q�6�   ��5#      �� 5$      	 �� Color$	 White��6"   .+�5&      �� .+�5'      �� -   5        $ TitleChampionship��5'      $��Cambria)�67   $��28(�6L   $ Center=�6Z   $��10q�6q   $��Center=�6}   $��WrapWithOverflow=�6�   $��Championship��6�   ���#0f0f0f$	��Top=�6�   �� 5(      -   5        $ TeamsChampionship��5(      $��Cambria)�67   $��20(�6L   $ Center=�6Z   $��10q�6q   $��Center=�6}   $
��Wrap=�6�   $��24 teams��6�   ���#121212$	��Top=�6�   5)      5*      � 5+   
   -   5        $ 
League One��5+   )   $��1��6:   $��3��6G   $��Stretch=�6W   $��Stretch=�6u   $��10, 10q�6�   ��5,      �� 5-      $	 White��6"   .+�5/      �� .+�50      �� -   5        $ TitleLeagueOne��50      $��Cambria)�64   $��28(�6I   $ Center=�6W   $��10q�6n   $��Center=�6z   $
��Wrap=�6�   $��
League One��6�   ���#0f0f0f$	��Top=�6�   �� 51      -	   5        $ TeamsLeagueOne��51      $��Cambria)�64   $��20(�6I   $ Center=�6W   $��10q�6n   $��Center=�6z   $
��Wrap=�6�   $��24 teams��6�   ���#121212$	��Top=�6�   52      53      � 54   
   -
   5        $ 
League Two��54   )   $��1��6:   $��4��6G   $��Stretch=�6W   $��Stretch=�6u   $��10, 10q�6�   ��55      �� 56      $	 White��6"   .+�58      �� .+�59      �� -   5        $ TitleLeagueTwo��59      $��Cambria)�64   $��28(�6I   $ Center=�6W   $��10q�6n   $��Center=�6z   $
��Wrap=�6�   $��
League Two��6�   ���#0f0f0f$	��Top=�6�   �� 5:      -   5        $ TeamsLeagueTwo��5:      $��Cambria)�64   $��20(�6I   $ Center=�6W   $��10q�6n   $��Center=�6z   $
��Wrap=�6�   $��24 teams��6�   ���#121212$	��Top=�6�   5;      5<      � 5=   
   -   5        $ Scottish Premier League��5=   )   $��2��6G   $��1��6T   $��Stretch=�6d   $��Stretch=�6�   $��10, 10q�6�   ��5>      �� 5?      $	 White��6"   .+�5A      �� .+�5B      �� -   5        $ TitleScottishPremiership��5B      $��Cambria)�6>   $��28(�6S   $ Center=�6a   $��10q�6x   $��Center=�6�   $
��Wrap=�6�   $��Scottish Premier League��6�   ���#0f0f0f$	��Top=�6�   �� 5C      -   5        $ TeamsScottishPremiership��5C      $��Cambria)�6>   $��20(�6S   $ Center=�6a   $��10q�6x   $��Center=�6�   $
��Wrap=�6�   $��12 teams��6�   ���#121212$	��Top=�6�   5D      5E      � 5F   
   -   5        $ Scottish Championship��5F   )   $��2��6E   $��2��6R   $��Stretch=�6b   $��Stretch=�6�   $��10, 10q�6�   ��5G      �� 5H      $	 White��6"   .+�5J      �� .+�5K      �� -   5        $ TitleScottishChampionship��5K      $��Cambria)�6?   $��28(�6T   $ Center=�6b   $��10q�6y   $��Center=�6�   $
��Wrap=�6�   $��Scottish Championship��6�   ���#0f0f0f$	��Top=�6�   �� 5L      -   5        $ TeamsScottishChampionship��5L      $��Cambria)�6?   $��20(�6T   $ Center=�6b   $��10q�6y   $��Center=�6�   $
��Wrap=�6�   $��10 teams��6�   ���#121212$	��Top=�6�   5M      5N      5P      5Q      