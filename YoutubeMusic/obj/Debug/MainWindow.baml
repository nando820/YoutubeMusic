   M S B A M L   `   `   `  ����G  CYoutubeMusic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:YoutubeMusicYoutubeMusic  5   	       YoutubeMusic.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:YoutubeMusic     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � $	��424��6   $��0,0,6,-0.333q�6   $	��Top=�6.   .+�5
   
   �� -   5         ��Name$ youtube_textbox��5
      $
��Left=�6+   $��28��6F   $��130,164,0,0q�6R    �� TextWrapping$
 Wrap=�6g   $	��Top=�6{   $	��468��6�   �� 5   
   -   5        $ destination_textBox��5      $
��Left=�6/   $��23��6J   $��130,259,0,0q�6V   $
 Wrap=�6k   $	��Top=�6   $	��468��6�   $5��/C:\Users\ferya\Music\iTunes\iTunes Media\Music\��6�   �� 5   
   -   5         ��Name$ convert_button��5      $�� ��6)   $
��Left=�6Q   $��625,156,0,0q�6l   $	��Top=�6�   $��59��6�   $��45��6�    J� RenderTransformOrigin$ 	0.491,0.4&�6�   $��14(�6�   $��Bernard MT Condensed)�6�   ���	#FFF9F6F6��5      �� 5       �� ImageSource$ download.png��6   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 0.483Q�6&   T� 5       T� X$ 0.038Q�6)    T� Y$ -0.004Q�63   5      5      �� 5   
   -   5        	 ��Name$	 imageBox��5      $
��Left=�6"   $	��114��6=   $��
161,27,0,0q�6J   $	��Top=�6^   $	��125��6v   $��youtube.png��6�   �� 5   
   -   5        
 ��Name$
 music_label��5      $��YouTube MP3 Converter��6%   $
��Left=�6E   $��
291,44,0,0q�6`   $	��Top=�6t   $��Cooper Black)�6�   $��50��6�   $	��318��6�   $��24(�6�   �� 5   
   -   5        $
 
link_label��5      $��Link de YouTube��6$   $
��Left=�6?   $��130,132,0,0q�6Z   $	��Top=�6o   �� 5   
   -   5        $
 destination_label��5      $��Folder de Destino��6+   $
��Left=�6G   $��130,233,0,0q�6b   $	��Top=�6w   $ -1.895,-0.263&�6�   $	��156��6�   �� 5   
   -   5        $ destination_button��5      $��Buscar Carpeta��6-   $
��Left=�6g   $��626,262,0,0q�6�   $	��Top=�6�   $��93��6�   � 5   
   -	   5         �Name$ progressbar1��5      $
��Left=�6,   $��22��6G   $��154,339,0,0q�6S   $	��Top=�6h   $	��444��6�   �� 5   
   -
   5        $ Cancel_button��5      $��Cancelar��6(   $
��Left=�6W   $��343,384,0,0q�6r   $	��Top=�6�   $��75��6�    �� IsCancel . 6�   �� 5    
   -   5        $
 download_label��5       $��	Descargar��6(   $
��Left=�6<   $��626,132,0,0q�6W   $	��Top=�6l   �� 5!   
   -   5        $
 status_label��5!      $��Status��6&   $
��Left=�67   $��154,309,0,0q�6R   $	��Top=�6g   $	��432��6   $��25��6�   5#      5$      