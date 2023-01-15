import {apiUrl} from "../App";

export let CreateNoteRequest = (setIsLoading, jwt, title, description, setIsSuccess, setError) => {
    fetch(`${apiUrl}/Note/AddNote`,
        {
            "mode": "cors",
            "method": "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                Authorization: `Bearer ${jwt}`
            },
            body: JSON.stringify(
                {
                    name: title,
                    description: description,
                }
            )
        }
    ).then(response => {
        setIsLoading(false)
        if (response.status === 200) {
            setIsSuccess(true)
        } else {
            setError(response.status);
        }
    })
        .catch(e => {
            setError(e.toString());
        });
}