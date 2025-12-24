	The InPut.txt file must exist in the application folder as a CSV file in the following format
OrigCity, OrigState, OrigZip, DestCity, DestState, DestZip, FlatRate
Zip Codes are optional but include spaces if not available.  Miles lookup is more accurate with zip codes.

	The applicaiton will process the data; lookup the miles and calculate the rate per mile and write
the data to the OutPut.txt file.  the File will always be replaced.  The OutPut.txt format is:
OrigCity, OrigState, OrigZip, DestCity, DestState, DestZip, FlatRate, Miles, MileRate,Message
Messages are added if something goes wrong.

Examples:
InPut.txt
Chicago,IL,60606,Inverness,IL,60010,200
Inverness,IL,60010,Chicago,IL,60606,400

OutPut.txt
Chicago,IL,60606,Inverness,IL,60010,200,37,5.41,
Inverness,IL,60010,Chicago,IL,60606,400,38,10.53,
Chicago,IL,60606,Inverness,IL,60010,200,37,5.41,
Inverness,IL,60010,Chicago,IL,60606,400,38,10.53,