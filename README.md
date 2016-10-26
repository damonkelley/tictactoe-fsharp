<h1>TicTacToe - F#</h1>

An unbeatable Tic Tac Toe game written in F#


### Setup

First, install Mono.

```sh
$ brew install mono
```

Then, download and build the project.

```sh
$ git clone https://github.com/damonkelley/tictactoe-fsharp.git
$ cd tictactoe-fsharp
$ ./build.sh
```

### Fake

Fake is the build tool used for the project. All targets are defined in `build.fsx`. The `build.sh` script is a helper that will invoke Fake.

Invoking the script with no arguments will execute the default target which will compile and test the project.

```sh
$ ./build.sh
```

### Play the game

```sh
$ mono ./build/TicTacToe.exe
```

### Tests

Run the unit tests.

```sh
$ ./build.sh Test
```

Run the integrations tests. This will run two tests that test the computer strategy. These take approximately 2 minutes to complete.

```sh
$ ./build.sh LongTests
```

Run the unit tests and the long tests.

```sh
$ ./build.sh AllTests
```
