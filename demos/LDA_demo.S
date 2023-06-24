    .org $8000

reset:
    lda #$ff
    sta $6002

    lda #$50
    sta $6000

loop:
    ror
    sta $6000
    jmp loop

irq:
    rti

nmi:
    rti

    .org $fffa
    .word nmi
    .word reset
    .word irq
;    .word $8000
