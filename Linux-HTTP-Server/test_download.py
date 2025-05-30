import requests

SERVER_URL = "http://192.168.66.18:5000/download/1"  # Replace with your server's address
DOWNLOAD_PATH = "downloaded_books.zip"  # Path to save the downloaded file


def download_file():
    """Download the file from the server."""
    print("Connecting to the server to download the ZIP file...")
    try:
        response = requests.get(SERVER_URL, stream=True)
        if response.status_code == 200:
            with open(DOWNLOAD_PATH, "wb") as file:
                for chunk in response.iter_content(chunk_size=8192):
                    file.write(chunk)
            print(f"File downloaded successfully: {DOWNLOAD_PATH}")
        else:
            print(f"Failed to download file. Server returned status code {response.status_code}")
    except requests.exceptions.RequestException as e:
        print(f"Error while downloading file: {e}")


def main():
    response =requests.get("http://192.168.66.18:5000/list_books")
    print(response.json())
    print("Download Test Script")
    download_file()


if __name__ == "__main__":
    main()