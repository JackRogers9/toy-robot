# Test data

Sample command sequences fed to the app on stdin. Each file ends on its own
(end of input), and only `REPORT` lines are printed.

```bash
dotnet run --project src/ToyRobot.Cli < test-data/example-a.txt
```

## Specification examples

| File | Commands | Expected output |
|---|---|---|
| `example-a.txt` | PLACE 0,0,NORTH / MOVE / REPORT | `0,1,NORTH` |
| `example-b.txt` | PLACE 0,0,NORTH / LEFT / REPORT | `0,0,WEST` |
| `example-c.txt` | PLACE 1,2,EAST / MOVE / MOVE / LEFT / MOVE / REPORT | `3,3,NORTH` |

## Edge cases

| File | What it exercises | Expected output |
|---|---|---|
| `edge-commands-before-place.txt` | Commands before a valid PLACE are discarded | `1,1,SOUTH` |
| `edge-fall-prevention.txt` | Off-edge moves ignored; later valid moves still run | `0,0,SOUTH`, `1,0,EAST`, `4,4,NORTH` |
| `edge-invalid-input.txt` | Garbage, off-table PLACE (9,9), bad direction, missing/extra args all ignored | `1,2,WEST` |
