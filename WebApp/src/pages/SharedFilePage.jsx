import { useParams } from "react-router-dom";
import {downloadFile, fileInfo} from "../utils/api/Files.js";
import { useEffect, useState } from "react";
export default function SharedFilePage() 
{
    const params = useParams();
    const {shareToken} = useParams();
    const [error, setError] = useState(null);
    const [info, setInfo] = useState(null);
    useEffect(() => 
    {
        async function fetchFileInfo()
        {
            try
            {

                const data = await fileInfo(shareToken);
                setInfo(data);
                console.log(fileInfo);
            }
            catch (error)
            {
                console.error('Error fetching file info:', error);
                setError('Failed to fetch file info');
            }
        }

        fetchFileInfo();
    }, [shareToken]);

    return (
            <div>
                <h1>{info?.fileName}</h1>
                <p>Size: {info?.fileSize} bytes</p>
                <p>Uploaded at: {info?.uploadedAt}</p>
                <p>Expires at: {info?.expiresAt}</p>
                {error && <p style={{ color: 'red' }}>{error}</p>}
            <button onClick={() => downloadFile(shareToken)}>
                Download
            </button>
        </div>
    );
}