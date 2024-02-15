export function dateToFormattedString(stringDate) {
    let date = new Date(stringDate);
    let d = date.getDate();
    let m = date.getMonth() + 1;
    let y = date.getFullYear();
    let mm = date.getMinutes();
    let hh = date.getHours();
    return '' + (d <= 9 ? '0' + d : d) + '.' + (m<=9 ? '0' + m : m) + '.' + y +' '+(hh <= 9 ? '0' + hh : hh)+':'+(mm <= 9 ? '0' + mm : mm);
}
export function datePickConvert(stringDate){
    let date = new Date(stringDate);
    let d = date.getDate();
    let m = date.getMonth() + 1;
    let y = date.getFullYear();
    return '' + (d <= 9 ? '0' + d : d) + '.' + (m<=9 ? '0' + m : m) + '.' + y;
}