import csv 
import os
import random
from datetime import datetime, timedelta
import struct

# === SETTINGS ===
NUM_SENSORS = 10
DAYS = 30
HOURS = 24
SENSOR_TYPE = "Temperature"
OUTPUT_FOLDER = os.path.join(os.path.dirname(__file__), "Datasets")

# Create output folder if it doesn't exist
os.makedirs(OUTPUT_FOLDER, exist_ok=True)

# Date range: from 29 days ago up to today
today = datetime.now().replace(hour=0, minute=0, second=0, microsecond=0)
start_date = today - timedelta(days=DAYS - 1)

# Convert datetime to .NET ticks
def to_dotnet_ticks(dt: datetime) -> int:
    dotnet_epoch = datetime(1, 1, 1)
    delta = dt - dotnet_epoch
    return delta.days * 864000000000 + delta.seconds * 10000000 + delta.microseconds * 10

# === GENERATE ONE FILE PER SENSOR ===
for sensor_id in range(1, NUM_SENSORS + 1):
    sensor_name = f"Sensor{sensor_id}"
    csv_path = os.path.join(OUTPUT_FOLDER, f"{sensor_name}.csv")
    bin_path = os.path.join(OUTPUT_FOLDER, f"{sensor_name}.bin")

    # Generate full month grid for this sensor
    data_rows = []  # for CSV
    binary_records = []  # for BIN

    for day_offset in range(DAYS):
        current_date = start_date + timedelta(days=day_offset)
        date_str = current_date.strftime("%Y-%m-%d")
        row = [date_str]

        for hour in range(HOURS):
            timestamp = current_date + timedelta(hours=hour)
            value = round(random.uniform(0.0, 1.0), 2)
            row.append(value)
            binary_records.append((sensor_id, SENSOR_TYPE, timestamp, value))

        data_rows.append(row)

    # === WRITE CSV (Grid format) ===
    with open(csv_path, "w", newline="") as f_csv:
        writer = csv.writer(f_csv)
        writer.writerow(["Date"] + [f"hour {h}" for h in range(HOURS)])
        writer.writerows(data_rows)

    # === WRITE BIN (Record format) ===
    with open(bin_path, "wb") as f_bin:
        for record in binary_records:
            f_bin.write(struct.pack('<i', record[0]))  # ID
            f_bin.write(record[1].encode('utf-8') + b'\x00')  # Type
            f_bin.write(struct.pack('<q', to_dotnet_ticks(record[2])))  # Ticks
            f_bin.write(struct.pack('<d', record[3]))  # Value

    print(f"✅ Saved {sensor_name}.csv and {sensor_name}.bin in {OUTPUT_FOLDER}")

