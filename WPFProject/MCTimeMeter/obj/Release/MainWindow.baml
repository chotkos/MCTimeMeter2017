   M S B A M L   `   `   `  ����   MCTimeMeter    MCTimeMeter.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     �Name$  MainWindow1�� � Title$ MCTimeMeter��$	��450��$	��500�� � Icon$ logo.ico��.��� 
���   � � RowDefinitions �� $��72 ��� $��50 ��� $��10 ��� $��50 ��� $��10 ��� $��75 ��� �� $��25 � � ColumnDefinitions �� $
��Auto ��� $
��Auto �.+��� -    ��Name$ TimersScroll��
������ $��0,0,0,0q�
�������
���   �$��7��$
��Auto=�.���� -    ��Name$ 
TimersList��$��10q�
������ $
��Auto=��� -    ��Name$ 
StopButton��$
��Stop��$��1��$��17,10,15,10q�
������ 
�������
�������$	��120���� -    ��Name$ TaskComboBox��$��1��$��	17,0,15,0q�$��3��$	��Top=�
������ 
�������
���   �$��22��$	��120���� -   $ AddTaskButton��$��Dodaj zadanie��$��3��$��1��$��
17,28,15,0q�
������ 
�������
�������$��22��$	��120���� -   $ SetUserButton��$��Ustaw Użytkownika��$��1��$��1��$��
17,28,15,0q�
������ 
�������
�������$��22��$	��120���� -   	 ��Name$	 UserNameTextBox��$��1��$��23��$��	17,0,15,0q�$��1��
 �� TextWrapping$
 NoWrap=�$��Kod użytkownika��$	��Top=�$��Center=�$��Center=�$	��120���� $��	logo3.png��$��1��$��Center=�$	��125��$��	13,0,16,0q�$��6��$��Bottom=�$	��124���� $��Mateusz Chodkowski 2014��$��1��$��Center=�$��7��$	��Top=�$��0,0,0,0q�$��Center=�
�������