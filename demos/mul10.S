    .org $8000

main:
    ldx #0
    ldy #0
    lda #10
    jsr mult10
    sta $d020
    jmp main

mult10:
    asl
    sta temp
    asl
    asl
    clc
    adc temp
    rts
    
temp:
    .byte 0

    .org $fffc
    .word main
    .word $8000