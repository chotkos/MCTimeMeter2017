   M S B A M L   `   `   `  ����   TimePanelControl"    TimePanelControl.TimeControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $	��120��
�������$��1q�.��� -     �Name$  TimeGrid��$
��Left=�$	��120��$	��Top=�$	��300��
������  � ColumnDefinitions �� $��2* ��� $��2* ��� ��  � RowDefinitions �� $	��25* ��� $	��50* ��� $	��25* �.+��� -    ��Name$ WorkItemNameLabel��$(��"Work Item #8889 - Client: Kowalski��$��10,2,0,0q�$	��Top=�$��28��$��3��
��������� -   $ 
TimerLabel��$��00:00:00��$
��10,0q�$��1��
�������$��24(�$��Center=�$��Center=�$��2���� -    ��Name$ MinutesTextBox��$��2�� �� TextWrapping$
 Wrap=�$�� ��
������ 
������� �� 
CaretBrush
 �����$��1��$��Center=�$	��0,5q��� -   $ 
DescrLabel��$��Minuty:��$��Right=�$��15,2,0,0q�$��1��
�������$��2��$��0���� -    ��Name$ StartButton��$��2��$��Start��$��2��$��5,5,5,0q�$��1��
������ 
�������
��������� -   $ AddMinutesButton��$��+��$��2��$��5,5,0,5q�$��2��
������ 
�������
��������� -   $ SubMinutesButton��$��-��$��3��$��0,5,5,5q�$��2��
������ 
�������
��������� -	   $ CloseButton��$�� ��$��3��$
��Left=�$	��0,5q�$��45��
������ 
�����������  �� ImageSource$ Close-128.png��	 j� Stretch$	 Uniform=�