import {apiUrl} from "../App";

export let GetPrivateNotesRequest = (jwt, setNotes, setError) => {
    fetch(`${apiUrl}/Note/GetNotes`,
        {
            "mode": "cors",
            "method": "GET",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${jwt}`
            },
        }
    ).then(response => {
        if (response.status === 200) {
            response.json().then(json => {
                setNotes(json)
            })
        } else {
            try {
                response.json().then(json => {
                    setError(json)
                })
            } catch {
                setError(response.status);
            }
        }
    })
        .catch(e => {
            setError(e.toString());
        });

}
