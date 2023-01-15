import {apiUrl} from "../App";

export let GetPublicNoteRequest = (id, setNote, setError) => {
    fetch(`${apiUrl}/PublicNote/GetNote?id=${id}`,
        {
            "mode": "cors",
            "method": "GET",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        }
    ).then(response => {
        if (response.status === 200) {
            response.json().then(json => {
                setNote(json)
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
