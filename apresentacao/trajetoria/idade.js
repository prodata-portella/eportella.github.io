(function age() {
    const calculate = (birthDate) => {
        const today = new Date()
        const birth = new Date(birthDate)
        let age = today.getFullYear() - birth.getFullYear()
        if (today.getMonth() < birth.getMonth() || (today.getMonth() === birth.getMonth() && today.getDate() < birth.getDate())) age--
        return age
    };
    (function ewerton() {
        document.getElementById('ewerton-age').textContent = calculate('1985-06-28')
    })();

    (function portella() {
        document.getElementById('portella-age').textContent = calculate('2016-06-24')
    })();
})();