import requests

URL = "http://127.0.0.1:5000"
#URL = "http://192.168.66.18:5000"  # Replace with your server's address
PATH = "downloaded_books/"  # Path to save the downloaded file


def download_file(SERVER_URL,DOWNLOAD_PATH):
    """Download the file from the server."""
    print("Connecting to the server to download the ZIP file...")
    try:
        print(SERVER_URL,DOWNLOAD_PATH)
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
    print("Download Test Start downloading book previews")
    download_file(URL+"/preview_books",PATH+"preview.csv")
    print("Download Test Start downloading Cool Book")
    download_file(URL+"/download/Cool_Book",PATH+"book.zip")


if __name__ == "__main__":
    main()