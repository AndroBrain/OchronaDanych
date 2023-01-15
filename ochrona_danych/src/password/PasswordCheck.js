export function CheckIsPasswordValid(password) {
    const regex = new RegExp("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*])(?=.{8,})")
    return regex.test(password)
}

export function CheckPasswordStrength(password) {
    const len = password.length
    const frequencies = Array.from(password)
        .reduce((freq, c) => (freq[c] = (freq[c] || 0) + 1) && freq, {})
    const strength = Object.values(frequencies)
        .reduce((sum, f) => sum - f / len * Math.log2(f / len), 0)
    if (strength < 2) {
        return "weak"
    } else if (strength > 4) {
        return "strong"
    } else {
        return "medium"
    }
}
