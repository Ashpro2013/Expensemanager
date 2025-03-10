//local storage
function setLocalData(val, key) {
    localStorage.setItem(key, val);
}
function getLocalData(key) {
    return localStorage.getItem(key);
}
function removeLocalData(key) {
    return localStorage.removeItem(key);
}
//local storage - end