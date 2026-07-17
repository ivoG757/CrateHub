    export async function uploadFile(token, file)
        {
            if(!file)
            {
                throw new Error("no file selected");
            }

                const formData = new FormData();
                formData.append("file", file);

                const response = await fetch('http://localhost:3000/api/UploadFile', 
                { 
                    method: "POST",
                    headers: 
                    {
                        Authorization: `Bearer ${token}`
                    },

                    body: formData
                });

                const data = await response.json();

                if (!response.ok)
                {
                    throw new Error(data.message);
                }

                return data;
        }
    
    export async function loadFiles(token)
        {
            const response = await fetch('http://localhost:3000/api/files',
            {
                method: "GET",
                headers:
                {
                    "Authorization": `Bearer ${token}`
                }
            });
            const data = await response.json();

            if (!response.ok)
            {
                throw new Error(data.message)
            }

            return data
        }