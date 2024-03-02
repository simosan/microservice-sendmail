export const handleToDate = (date) => {
    if (date !== "1900-01-01T00:00:00+09:00"){
        date = new Date(date);
        const formattedMinutes = date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes();
        const formattedSeconds = date.getSeconds() < 10 ? `0${date.getSeconds()}` : date.getSeconds();
        return `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()} ${date.getHours()}:${formattedMinutes}:${formattedSeconds}`;
    }
};

export const handleToTimestamp = (date) => {
    const utcYear = date.getUTCFullYear();
    const utcMonth = (date.getUTCMonth() + 1 < 10) ? `0${date.getUTCMonth() + 1}` : `${date.getUTCMonth() + 1}`;
    const utcDate = (date.getUTCDate() < 10) ? `0${date.getUTCDate()}` : `${date.getUTCDate()}`;
    const utcHours = (date.getUTCHours() < 10) ? `0${date.getUTCHours()}` : `${date.getUTCHours()}`;
    const utcMinutes = (date.getUTCMinutes() < 10) ? `0${date.getUTCMinutes()}` : `${date.getUTCMinutes()}`;
    const utcSeconds = (date.getUTCSeconds() < 10) ? `0${date.getUTCSeconds()}` : `${date.getUTCSeconds()}`;

    return `${utcYear}-${utcMonth}-${utcDate}T${utcHours}:${utcMinutes}:${utcSeconds}`;
};