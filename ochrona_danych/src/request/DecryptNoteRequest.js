import {apiUrl} from "../App";

export let DecryptNoteRequest = (jwt, id, password, note, setNote, setError, setIsLoading) => {
    fetch(`${apiUrl}/Note/DecryptNote`,
        {
            "mode": "cors",
            "method": "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwt}`
            },
            body: JSON.stringify(
                {
                    id: id,
                    password: password,
                }
            )
        }
    ).then(response => {
        setIsLoading(false)
        if (response.status === 200) {
            response.json().then(json => {
                setNote({
                    ...note,
                    description: json
                })
            })
        } else {
            setError(response.status);
        }
    })
        .catch(e => {
            setError(e.toString());
        });
}
