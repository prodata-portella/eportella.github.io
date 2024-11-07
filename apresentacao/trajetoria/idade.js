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
    const profissionalExperiencia = calculate('2001-01-01')
    const tiExperiencia = calculate('2012-01-01')

    document.getElementById('ewerton-idade').textContent = ewerton
    document.getElementById('portella-idade').textContent = portella
    document.getElementById('idade-total').textContent = ewerton;
    document.getElementById('idade-contato').textContent = contato;
    document.getElementById('idade-profissional-experiencia').textContent = profissionalExperiencia;
    document.getElementById('idade-ti-experiencia').textContent = tiExperiencia;

})();