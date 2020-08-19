Contains four classes :
1)LogParser :- tester class. Contains main() method;
2)Log :-  It is POCO for representing log from '*.csv' file.
3)UserInput :- Singleton class which holds user given input and does processing on it before use.
            (To access same instance anywhere)
4)FileReadWrite :- Provide ease in writing and reading '*.log' and '*.csv' file.

Additional features :
1) User can run by only giving values instead giving label for each value.But sequence must be as follows
        eg. dotnet run <source> <levels> <destination>
2) Also user can run it by giving source and destination only.In this case all levels will be considered.
        eg. dotnet run <source> <destination>
3) Also user can run it by giving any two labels with in any sequence of input values. Tow labels are must.
        eg. dotnet run level1 --log-dir <source> Level2 --csv <destination> Level3
4) Wrong levels will be ignored.
        eg. dotnet run <source> wrong level anything Info <destination> ...in this case only INFO will be considered.
5) Even If user run multiple time with same destination file, all output will appended to its previous output with   proper log index.

6) Header(<| No | Level | Date | Time | Text |>) will be added only at first line.

7) Blank lines and not in format lines will be skipped and skipped line count will shown after execution.

8) Log with Invalid date and time will not be considered.

9) If destination dir structure is not present it will be created automatically.


