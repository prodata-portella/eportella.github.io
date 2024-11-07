(function age() {
    const calculate = (birthDate) => {
        const today = new Date()
        const birth = new Date(birthDate)
        let age = today.getFullYear() - birth.getFullYear()
        if (today.getMonth() < birth.getMonth() || (today.getMonth() === birth.getMonth() && today.getDate() < birth.getDate())) age--
        return age
    };
    const ewerton = calculate('1985-06-28')
    const portella = calculate('2016-06-24')
    const contato = calculate('1995-06-28')
    const experiencia = calculate('2012-01-01')

    document.getElementById('ewerton-age').textContent = ewerton
    document.getElementById('portella-age').textContent = portella
    document.getElementById['age-total'].textContent = ewerton;
    document.getElementById['age-contato'].textContent = contato;
    document.getElementById('age-experiencia').textContent = experiencia;

})();