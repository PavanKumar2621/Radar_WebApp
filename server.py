import math
import socket
import time
import struct

# Network configuration
TARGET_IP = "127.0.0.1"
TARGET_PORT = 5005

# Packet Format definition using struct:
# > = Big-endian
# B = 1 byte unsigned char (Header, Cmd ID, Footer)
# i = 4 byte integer (Track ID)
# f = 4 byte float (Azimuth, Elevation, Range, Velocity, SNR)
# Format breakdown: Header(B), Cmd(B), Track(i), Az(f), El(f), Range(f), Vel(f), SNR(f), Footer(B)
# B = 1 byte unsigned char (used for Header, Cmd ID, Track ID, and Footer)
PACKET_FORMAT = "<B B B f f f f f B" 


def get_radar_data(current_time):
    """Generates the animated radar data based on your math logic."""
    return {
        "track_id": 1,
        "azimuth": 180.0 + 30.0 * math.sin(current_time),
        "elevation": 45.0 + 10.0 * math.sin(current_time * 0.5),
        "range": 2000.0 + 500.0 * math.sin(current_time * 0.2),
        "velocity": 50.0 + 10.0 * math.sin(current_time),
        "snr": 30.0 + 5.0 * math.sin(current_time * 0.7)
    }

def main():
    # Create a UDP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    
    sim_time = 0.0
    print(f"Streaming radar data to {TARGET_IP}:{TARGET_PORT}... Press Ctrl+C to stop.")
    c = 0
    try:
        while True:
            # 1. Fetch the data state
            data = get_radar_data(sim_time)
            # data["track_id"] = (int(sim_time * 20) % 6) + 1
            # 2. Pack the data into the exact binary structure
            # Header = 0x01, Cmd ID = 0x05, Footer = 0x09
            packet = struct.pack(
                PACKET_FORMAT,
                0x01,
                0x05,
                data["track_id"],
                data["azimuth"],
                data["elevation"],
                data["range"],
                data["velocity"],
                data["snr"],
                0x09
            )
            
            # 3. Send the raw bytes over UDP
            # if c < 2:
            #     c += 1
            sock.sendto(packet, (TARGET_IP, TARGET_PORT))
            print(packet.hex('-').upper())
            print(f"Sent packet: Track ID={data['track_id']}, Az={data['azimuth']:.2f}, El={data['elevation']:.2f}, Range={data['range']:.2f}, Vel={data['velocity']:.2f}, SNR={data['snr']:.2f}")
            # else:
            #     pass
            # Increment simulation variables matching your code
            sim_time += 0.1
            time.sleep(0.1) # Wait 100ms before sending the next frame
            
    except KeyboardInterrupt:
        print("\nStreaming stopped.")
    finally:
        sock.close()

if __name__ == "__main__":
    main()



# 01-05-00-00-00-01-43-36-FE-B8-42-35-FF-C9-44-FB-3F-FB-42-4B-FE-4B-41-F2-CC-37-09
# 01-05-00-00-00-01-43-36-FE-B8-42-35-FF-C9-44-FB-3F-FB-42-4B-FE-4B-41-F2-CC-37-09