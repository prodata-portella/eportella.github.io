(function blockquote() {
    const format = () => {
        document.querySelectorAll('blockquote p').forEach(p => {
            if (p.textContent === '[!NOTE]') {
                p.textContent = ''
                p.style.display = 'flex'
                p.style.alignItems = 'center'
                p.style.columnGap = '0.4em'
                const ns = "http://www.w3.org/2000/svg"

                const note = document.createElementNS(ns, "svg")
                note.setAttribute("viewBox", "0 0 16 16")
                note.setAttribute("version", "1.1")
                note.setAttribute("width", "16")
                note.setAttribute("height", "16")
                note.setAttribute("aria-hidden", "true")

                const path = document.createElementNS(ns, "path")
                path.setAttribute("d", "M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8Zm8-6.5a6.5 6.5 0 1 0 0 13 6.5 6.5 0 0 0 0-13ZM6.5 7.75A.75.75 0 0 1 7.25 7h1a.75.75 0 0 1 .75.75v2.75h.25a.75.75 0 0 1 0 1.5h-2a.75.75 0 0 1 0-1.5h.25v-2h-.25a.75.75 0 0 1-.75-.75ZM8 6a1 1 0 1 1 0-2 1 1 0 0 1 0 2Z")

                note.appendChild(path)
                p.appendChild(note)
                p.append('Note')
            }
        })
    };
    format();
})()